param (
    [string]$Configuration,
    [string]$TargetPath,
    [string]$TargetDir,
    [string]$ProjectDir,
    [string]$ProjectName,
    [string]$VsModInfoGeneratorDir,
    [string]$VsModInfoGenerator,
    [string]$VsModPackagerDir,
    [string]$VsModPackager,
    [string]$SolutionDir
)

$ErrorActionPreference = 'Stop'

Write-Host "[$ProjectName] PostBuild Script Started. Configuration: $Configuration"

# Uncomment to enable full transcript log (optional)
# $logPath = Join-Path $TargetDir "postbuild-trace.log"
# Start-Transcript -Path $logPath -Force

$debugOutDir = Join-Path $SolutionDir ".debug\$ProjectName"
$releaseOutDir = Join-Path $SolutionDir ".releases\"

if ($Configuration -eq "Debug") {
    Write-Host "[$ProjectName] Running Debug packaging..."

    Push-Location $VsModInfoGeneratorDir
    Write-Host "[$ProjectName] Generating mod info using: $VsModInfoGenerator"
    & $VsModInfoGenerator -a $TargetPath -o $TargetDir
    Pop-Location

    Write-Host "[$ProjectName] Copying build output to: $debugOutDir"
    Copy-Item -Path "$TargetDir\*" -Destination $debugOutDir -Recurse -Force -ErrorAction SilentlyContinue

    if (Test-Path "$ProjectDir\_Includes") {
        Write-Host "[$ProjectName] Copying root _Includes..."
        Copy-Item -Path "$ProjectDir\_Includes\*" -Destination $debugOutDir -Recurse -Force -ErrorAction SilentlyContinue
    }

    Get-ChildItem "$ProjectDir\Features" -Directory -ErrorAction SilentlyContinue | ForEach-Object {
        $includes = Join-Path $_.FullName "_Includes"
        if (Test-Path $includes) {
            Write-Host "[$ProjectName] Copying feature includes from: $($includes)"
            Copy-Item -Path "$includes\*" -Destination $debugOutDir -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}

elseif ($Configuration -eq "Package") {
    Write-Host "[$ProjectName] Running Package packaging..."

    $includesTargetDir = Join-Path $TargetDir "_Includes"

    if (Test-Path "$ProjectDir\_Includes") {
        Write-Host "[$ProjectName] Copying root _Includes to: $includesTargetDir"
        Copy-Item -Path "$ProjectDir\_Includes\*" -Destination $includesTargetDir -Recurse -Force -ErrorAction SilentlyContinue
    }

    Get-ChildItem "$ProjectDir\Features" -Directory -ErrorAction SilentlyContinue | ForEach-Object {
        $includes = Join-Path $_.FullName "_Includes"
        if (Test-Path $includes) {
            Write-Host "[$ProjectName] Copying feature includes from: $($includes)"
            Copy-Item -Path "$includes\*" -Destination $includesTargetDir -Recurse -Force -ErrorAction SilentlyContinue
        }
    }

    Push-Location $VsModPackagerDir
    Write-Host "[$ProjectName] Running Mod Packager: $VsModPackager"
    & $VsModPackager -a $TargetPath
    Pop-Location

    if (Test-Path "$TargetDir\release") {
        $zips = Get-ChildItem "$TargetDir\release\*.zip" -ErrorAction SilentlyContinue
        foreach ($zip in $zips) {
            Write-Host "[$ProjectName] Copying packaged ZIP: $($zip.Name) → $releaseOutDir"
            Copy-Item -Path $zip.FullName -Destination $releaseOutDir -Force
        }
    } else {
        Write-Warning "[$ProjectName] No release folder found at: $TargetDir\release"
    }
}
else {
    Write-Host "[$ProjectName] No action taken. Configuration '$Configuration' is not recognised."
}

Write-Host "[$ProjectName] PostBuild Script Complete."

# Uncomment to close transcript log
# Stop-Transcript