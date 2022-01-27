
const api = 'https://localhost:44312/api/Recipes'

const searchForm = document.querySelector('#search-form');
const resultContainer = document.querySelector('#result-container')

const resultCardTemplate = document.querySelector('template');
const resultCard = resultCardTemplate.content.querySelector('.result-card');
const starterSection = document.querySelector('.starter-section');
const mainSection = document.querySelector('.main-course-section');
const dessertSection = document.querySelector('.dessert-section');

const courseOptions = searchForm.querySelector('#course-options');

resultCardTemplate.remove();

let ingredientSearch;
let dietFilter, courseFilter;
let sortingKey;
let params;
let limit = 4;
let recipes;
let sortByName, sortByDifficulty;

searchForm.onsubmit = async event => {
    event.preventDefault();
    ingredientSearch = searchForm.querySelector('#ingredient-search').value;
    dietFilter = searchForm.querySelector('#diet-filter-select').value;
    courseFilter = courseOptions.querySelector('input[name="course"]:checked').value;
    sortingKey = searchForm.querySelector('#sorting-select').value;

    if(sortingKey == 0){
        sortByName = true;
        sortByDifficulty = false;
    }
    else{
        sortByDifficulty = true;
        sortByName = false;
    }

    while (resultContainer.firstChild) {
        resultContainer.removeChild(resultContainer.lastChild);
      }

    // Make api call for recipes.
    recipes = await getRecipes();
    displayRecipes(recipes); 
}

function displayRecipes(recipes){
    for (const recipe of recipes) {
        // Clone html div for recipe result info.
        let section = resultCard.cloneNode(true);
        section.querySelector('h2').textContent = recipe.name;
        section.querySelector('h3').textContent = recipe.description;
        section.querySelector('button').onclick = async event => {
            
            if(recipe.courseCategory == 0){
                addToMenu(recipe, starterSection);
            }
            else if(recipe.courseCategory == 1){
                addToMenu(recipe, mainSection);                
            }
            else{
                addToMenu(recipe, dessertSection);
            }
        }
        resultContainer.appendChild(section);
    };
}

function addToMenu(recipe, section){
    section.querySelector('h3').textContent = recipe.name;
    section.querySelector('p').textContent = recipe.description;
}

async function getRecipes(){
    params = new URLSearchParams({
        diet: dietFilter,
        course: courseFilter,
        ingredient: ingredientSearch,
        sortByName: sortByName,
        sortByDifficulty: sortByDifficulty
    });
    const response = await fetch(api + '?' + params.toString());
    const json = await response.json();
    
    return json;
}