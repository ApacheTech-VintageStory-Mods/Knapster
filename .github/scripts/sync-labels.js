const fs = require("fs");
const path = ".github/labels.json";

// Check if the file exists and is readable
if (!fs.existsSync(path)) {
    throw new Error(`File not found: ${path}`);
}

const labels = JSON.parse(fs.readFileSync(path, "utf8"));

// Function to delete all existing labels
async function deleteAllLabels(github, context) {
    try {
        const existingLabels = await github.rest.issues.listLabelsForRepo({
            owner: context.repo.owner,
            repo: context.repo.repo
        });
        console.log(`Found ${existingLabels.length} labels.`);

        for (const label of existingLabels.data) {
            try {
                await github.rest.issues.deleteLabel({
                    owner: context.repo.owner,
                    repo: context.repo.repo,
                    name: label.name
                });
                console.log("Deleted label: ", label.name);
            } catch (error) {
                console.error(`Failed to delete label: ${label.name}`, error);
            }
        }
    } catch (ex) {
        console.error("Error fetching existing labels:", ex);
        throw ex;
    }
}

// Function to add labels from JSON file
async function addLabels(github, context) {
    for (const label of labels) {
        try {
            await github.rest.issues.createLabel({
                owner: context.repo.owner,
                repo: context.repo.repo,
                name: label.name,
                color: label.color,
                description: label.description
            });
            console.log("Added label: ", label.name);
        } catch (error) {
            if (error.status === 422) {
                // Log that the label already exists
                console.warn(`Label already exists: ${label.name}`);
            } else {
                console.error(`Error creating label: ${label.name}`, error);
                throw error;
            }
        }
    }
}

module.exports = async ({ github, context }) => {
    try {
        // Clear existing labels
        await deleteAllLabels(github, context);

        // Add new labels from JSON
        await addLabels(github, context);

        console.log("Labels updated successfully.");
    } catch (error) {
        console.error("Error updating labels:", error);
    }
};