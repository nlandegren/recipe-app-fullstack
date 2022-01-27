# Todo

## Graded G

### Database

* make database component
    * ~~add classes and properties    
    * ~~add relationships
    * add configurations
        * proper data types and constraints
    * ~~simplify quantity table 

* Recipe
    * ~~Enum conversion on MealCategory

* Ingredient
    * ~~Enum conversion on dietcategory

* IMPROVEMENTS
    * remove string conversions on enums, rename enum values

### Basic CRUD operations

* Recipes 
    * ~~List~~
    * Create
        * ~~Redirect to edit page to add ingredients~~
        * ~~Validation messages~~
        * Proper validation (data annotations on model)
    * ~~Delete~~
    * Edit         
        * ~~Edit/add/delete for each ingredient ingredients all ingredients~~
        * Proper validation (data annotations on model)
    * ~~Details~~
    * ~~Filter by meal category~~
        * ~~Fix placeholder in drop down~~
    * ~~Sort by number of ingredients~~
        * ~~Fix placeholder in drop down~~
    * ~~Search by name and description~~

* Ingredients
    * ~~List~~
    * ~~Create~~
        * ~~Change description to food category~~
    * ~~Delete~~
    * ~~Edit~~
    * ~~Details~~
    * ~~Filter by diet~~
        * ~~Fix placeholder in drop down~~
    * ~~Sort by name or popularity~~
        * ~~Fix placeholder in drop down~~
        * ~~probably fix LINQ expression~~
    * ~~Search by name~~


* MealPlans
    * ~~List
    * ~~Create
        * ~~Associate plan with recipe.
        * ~~Make it so you can add multiple days and recipes at the same time in the same form.~~
    * ~~Delete
    * Edit
        * ~~Fix so we can see all recipes and edit each.~~
    * ~~Details~~
        * ~~Group by week day~~
    * ~~Sort by week number~~
    * ~~Filter by diet category~~





### Authorization
    * ~~Edit/Delete recipe only allowed if userid == httpuserid
    * Write same code for mealplans and commoent out until userid in table
    
### Create sample data
    * ~~ingredients
    * ~~Recipes
    * ~~Units
    * ~~Quantities
    * MealPlans


### Html
    * Make sure all labels are in swedish and use Display(name) if possible


### ~~CSS~~
    * Tidy up if have time

### BUGS

    * cant add first weekday or ingredient because we skip index 0


## Graded VG

A page that lets the user create a three course meal by choosing diet option for the whole meal and for each course entering key word search. The user may save the meal to local storage.

* Go through the first assignment and the pixabay assignment and take notes.



### Notes

### User experience

* Start page says: Generate meals with up to three courses!
* User chooses what courses to include by checking or unchecking
* User chooses what diet to filter by with drop down list
* User enters keywords to search ingredients and recipes for
* User may sort by difficulty and/or name

### Database

* ~~Add course column to Recipe.~~
* ~~Add difficulty to recipe so that we can sort meals by total difficulty.~~
* ~~make default value difficulty 1~~

#### Sample data
* ~~Generate some random plans~~
* ~~Add Recipes so that we have at least 1 recipe per course category~~
* ~~set difficulty on recipes~~
* ~~set course category on recipes~~
    * add to edit form in pages
* Add Recipes so that we have at least 1 recipe per difficulty
* Add recipe so that we have at least 2 recipe per diet
* Make sample data where course category is set

### API

#### Make controller to get all recipes.
* ~~Get all recipes~~
* ~~Filter by recipe name keyword.~~
* ~~Filter by diet~~
* ~~Filter by course~~
* ~~Filtered by ingredient search word~~
* ~~Sort by difficulty~~~
* ~~Sort by name~~

#### Make controller to get ingredients for a single recipe

### Frontend

#### HTML
    * ~~main form for getting recipes~~
        * ~~change so that course option is radio panel~~
    * search result template
        * change so that it only lists recipes of one course type
    * make a menu section
    * add to menu button for each search result 



#### CSS
* JavaScript




## Process

* Set up database
* Added razor pages for tables
* made sure all required content was displayed on razor pages
    * On recipe details this was Ingredients, quantities and instruicions

    ## Problems

* database problems
        Note: Kopplade relational properties mellan Recipe och Ingredient, fick en automatisk join-tabell. Ändrade relational properties så att de pekade på Quantity istället, vilket gjorde Quantity till join-tabellen istället.
* linq include problems due to join table

### Recipe Create Page
Att lägga till ett recept innebär också att 1. lägga till ingredienser, och 2. att associera ingredienserna med receptet. 

Lösning 1. Det optimala vore att under recept-formuläret ha en sektion för att lägga till ingredinser till receptet i form av en textbox med autocomplete som föreslår ingredienser som redan finns i databasen. Om användaren väljer ett förslag, så läggs denna information in i Quantity-tabellen. Om användaren skriver in något helt nytt, så läggs det till en ny ingrediens i Ingredient-tabellen, samt länkas ingrediens till receptet med Quantity-tabellen.

Problem med Lösning 1: jag har inte tid att sätta mig in i autocomplete-verktyget.

Lösning 2. Istället för autocomplete, hämta in alla ingredienser i en drop down (man kan ju söka i drop downs). Om ingrediensen inte finns, så lägger vi till det i ett formulär under.

Lösningsförslag 3. På sidan för skapa nytt recept har jag tre formulär: ett för att skapa ett nytt recept (namn, beskrivning, instruktioner); ett för att skapa nya ingredienser; och ett för att lägga till ingredienserna till receptet (detta blir rader i tabellen quantity). 
        
Lösningsförslag 4. På sidan för skapa nytt recept har jag två formulär: ett för att skapa ett nytt recept och ett för att lägga till ingredienser till receptet. Om ingredienserna inte finns, så läggs de till i databasen. 




### Problem att ta upp med Jakob

* bättre formulär
    * ~~lägga till/redigera/ta bort ingredienser i recept~~
        * bugg som bara händer ibland (flera object av samma identitet som "trackas samtidigt")
    * lägga till/redigera/ta bort måltider i matplaneringar
* Gruppera måltider i matplanering efter dag
* Går det bra att jag skippar exempeldata på matplanering? Det är omständigt


Lösningar
    * whitespace css för radbryt, värde pre eller preline
    * en delete-knapp per ingrediens som visas i en statisk lista ovanför ingrediens fälten, gör handler i edit för att ta bort ingrediensen
    * om modelstate-problemet: 
FIXA SÅ ATT TOMMA FÄLT INTE SKICKAS MED FORMULÄRET

