const fs = require("fs");
const path = ".github/labels.json";
const labels = JSON.parse(fs.readFileSync(path, "utf8"));

// Function to delete all existing labels
async function deleteAllLabels(github, context) {
    const existingLabels = await github.rest.issues.listLabelsForRepo({
        owner: context.repo.owner,
        repo: context.repo.repo
    });

    for (const label of existingLabels.data) {
        await github.rest.issues.deleteLabel({
            owner: context.repo.owner,
            repo: context.repo.repo,
            name: label.name
        });
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
        } catch (error) {
            if (error.status !== 422) { // 422 means unprocessable entity, which could indicate the label already exists.
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