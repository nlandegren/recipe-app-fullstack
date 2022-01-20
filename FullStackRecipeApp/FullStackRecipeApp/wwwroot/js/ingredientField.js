function NewIngredientField() {

    const addIngredientTemplate = document.querySelector("template")

    const addIngredientDiv = addIngredientTemplate.querySelector("div")

    const addFieldButton = document.getElementById("ingredient-field-button")

    const submitButtonContainer = document.getElementById("submit-button-container")

    const recipeForm = document.getElementById("create-recipe-form")

    addFieldButton.onclick = event => {
        const clone = addIngredientDiv.cloneNode(true)
        recipeForm.insertBefore(clone, submitButtonContainer)
    };
}