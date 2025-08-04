param(
    [string]$ProjectName,
    [string]$ProjectDir,
    [string]$TargetDir,
    [string]$TargetPath,
    [string]$SolutionDir,
    [string]$ModId
)

$ErrorActionPreference = 'Stop'

# Ensure the environment variables are set
if (-not $env:VsModTranslationPatcherDir) {
    Write-Error "Environment variable 'VsModTranslationPatcherDir' is not set."
    exit 1
}
if (-not $env:VsModTranslationPatcher) {
    Write-Error "Environment variable 'VsModTranslationPatcher' is not set."
    exit 1
}
if (-not $env:VsModInfoGeneratorDir) {
    Write-Error "Environment variable 'VsModInfoGeneratorDir' is not set."
    exit 1
}
if (-not $env:VsModInfoGenerator) {
    Write-Error "Environment variable 'VsModInfoGenerator' is not set."
    exit 1
}
if (-not $env:VsModPackagerDir) {
    Write-Error "Environment variable 'VsModPackagerDir' is not set."
    exit 1
}
if (-not $env:VsModPackager) {
    Write-Error "Environment variable 'VsModPackager' is not set."
    exit 1
}
if (-not $env:VsModArtifactsDir) {
    Write-Error "Environment variable 'VsModArtifactsDir' is not set."
    exit 1
}

# Ensure the required parameters are provided
if (-not $ProjectName) {
    Write-Error "Parameter 'ProjectName' is required."
    exit 1
}
if (-not $ProjectDir) {
    Write-Error "Parameter 'ProjectDir' is required."
    exit 1
}
if (-not $TargetDir) {
    Write-Error "Parameter 'TargetDir' is required."
    exit 1
}
if (-not $TargetPath) {
    Write-Error "Parameter 'TargetPath' is required."
    exit 1
}
if (-not $SolutionDir) {
    Write-Error "Parameter 'SolutionDir' is required."
    exit 1
}
if (-not $ModId) {
    Write-Error "Parameter 'ModId' is required."
    exit 1
}

# Ensure the required directories exist
if (-not (Test-Path $ProjectDir)) {
    Write-Error "Project directory '$ProjectDir' does not exist."
    exit 1
}
if (-not (Test-Path $TargetDir)) {
    Write-Error "Target directory '$TargetDir' does not exist."
    exit 1
}
if (-not (Test-Path $SolutionDir)) {
    Write-Error "Solution directory '$SolutionDir' does not exist."
    exit 1
}

# Ensure the required files exist
$requiredFiles = @(
    "$env:VsModTranslationPatcherDir\$env:VsModTranslationPatcher",
    "$env:VsModInfoGeneratorDir\$env:VsModInfoGenerator",
    "$env:VsModPackagerDir\$env:VsModPackager"
)
foreach ($file in $requiredFiles) {
    if (-not (Test-Path $file)) {
        Write-Error "Required file '$file' does not exist."
        exit 1
    }
}

# Ensure the target path is valid
if (-not (Test-Path $TargetPath)) {
    Write-Error "Target path '$TargetPath' does not exist."
    exit 1
}
# Ensure the target directory exists
if (-not (Test-Path $TargetDir)) {
    Write-Error "Target directory '$TargetDir' does not exist."
    exit 1
}
# Ensure the solution directory exists
if (-not (Test-Path $SolutionDir)) {
    Write-Error "Solution directory '$SolutionDir' does not exist."
    exit 1
}
# Ensure the artifacts directory exists
if (-not (Test-Path $env:VsModArtifactsDir)) {
    Write-Error "Artifacts directory '$env:VsModArtifactsDir' does not exist."
    exit 1
}

# Ensure the artifacts directory for the project exists
$projectArtifactsDir = Join-Path $env:VsModArtifactsDir $ProjectName
if (-not (Test-Path $projectArtifactsDir)) {
    New-Item -ItemType Directory -Path $projectArtifactsDir | Out-Null
}

# Function to trace messages with indentation
function Trace {
    param(
        [int]$Indent,
        [string]$Message
    )
    $arrow = '→'
    $prefix = ' ' * ($Indent * 3)
    Write-Host " $prefix$arrow $Message"
}

Trace 0 ""
Trace 0 "======================================================================================="
Trace 0 " POST-BUILD EVENT FOR $ProjectName"
Trace 0 "======================================================================================="
Trace 0 ""

# Start the packaging process
Trace 1 "PostBuild: Packaging mod $ProjectName in $env:Configuration mode."

Trace 2 "Patching and Merging Mod Translation Files..."
& "$env:VsModTranslationPatcherDir\$env:VsModTranslationPatcher" -p "$ProjectDir" -t ([System.IO.Path]::GetDirectoryName($TargetPath)) -m $ModId -l Information

Trace 2 "Generating Mod Info File..."
& "$env:VsModInfoGeneratorDir\$env:VsModInfoGenerator" -a "$TargetPath" -o ([System.IO.Path]::GetDirectoryName($TargetPath))

Trace 2 "Merge Gantry into Mod assembly..."
$smartAssemblyPath = "C:\\Program Files\\Red Gate\\SmartAssembly 8\\SmartAssembly.com"

Trace 2 "Copying _Includes files to output folder..."
Copy-Item -Path "$ProjectDir\_Includes\*" -Destination "$TargetDir\_Includes" -Recurse -Force

Trace 2 "Copying Gantry _Includes files to output folder..."
Copy-Item -Path "$SolutionDir\.gantry\_Includes\*" -Destination "$TargetDir\_Includes" -Recurse -Force

Trace 2 "Copying Feature _Includes files to output folder..."
Copy-Item -Path "$ProjectDir\Features\*\_Includes\*" -Destination "$TargetDir\_Includes" -Recurse -Force

Trace 2 "Copying build output files to .debug folder..."
if (!(Test-Path "$SolutionDir\.debug\$ProjectName")) {
    New-Item -ItemType Directory -Path "$SolutionDir\.debug\$ProjectName" | Out-Null
}
Copy-Item -Path "$TargetDir\*" -Destination "$SolutionDir\.debug\$ProjectName" -Recurse -Force

# Move all files from _Includes to debug output root and remove _Includes folder
$includesPath = Join-Path "$SolutionDir\.debug\$ProjectName" "_Includes"
if (![string]::IsNullOrWhiteSpace($includesPath) -and (Test-Path $includesPath)) {
    Trace 2 "Moving all files from _Includes to debug output root and removing _Includes folder..."
    Get-ChildItem -Path $includesPath -Recurse | Move-Item -Destination "$SolutionDir\.debug\$ProjectName" -Force
    Remove-Item -Path $includesPath -Recurse -Force
}

Trace 2 "Packaging mod using VsModPackager..."
Push-Location $env:VsModPackagerDir
& "$env:VsModPackagerDir\$env:VsModPackager" -a "$TargetPath" -d "$SolutionDir\.debug\$ProjectName\"
Pop-Location

Trace 2 "Copying package to .releases folder..."
Copy-Item -Path "$TargetDir\release\*.zip" -Destination "$SolutionDir\.releases" -Force

Trace 2 "Copying package to $projectArtifactsDir..."
Copy-Item -Path "$TargetDir\release\*.zip" -Destination "$projectArtifactsDir" -Force

Trace 1 "Mod $ProjectName packaged successfully."
Trace 0 "Debug output: file://$SolutionDir.debug\$ProjectName"
Trace 0 "Package output: file://$SolutionDir.releases"
Trace 0 "Artifacts archive: file://$env:VsModArtifactsDir"
