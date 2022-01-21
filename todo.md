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


### Basic CRUD operations

* Recipes 
    * ~~List~~
    * Create
        * ~~Redirect to edit page to add ingredients~~
        * ~~Validation messages~~
        * Proper validation (data annotations on model)
    * ~~Delete~~
    * Edit         
        * Edit/add/delete for each ingredient ingredients all ingredients
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
        * Make dietcategory drop down
    * ~~Delete~~
    * ~~Edit~~
    * ~~Details~~
    * ~~Filter by diet~~
        * ~~Fix placeholder in drop down~~
    * ~~Sort by name or popularity~~
        * Fix placeholder in drop down
        * probably fix LINQ expression
    * ~~Search by name~~


* MealPlans
    * ~~List
    * ~~Create
        * ~~Associate plan with recipe.
        * Make it so you can add multiple days and recipes at the same time in the same form.
    * ~~Delete
    * Edit
        * Fix so we can see all recipes and edit each.
        * Edit adds new recipe??
    * Details
        * Group by week day
    * Sort by week number
    * Filter by diet category (majority of the meals need to be in a category)


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




## Graded VG



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

* Javascript för bättre formulär
    * lägga till/redigera/ta bort ingredienser i recept
    * lägga till/redigera/ta bort måltider i matplaneringar
* Gruppera måltider i matplanering efter dag
* Går det bra att jag skippar exempeldata på matplanering? Det är omständigt


Lösningar
    * whitespace css för radbryt, värde pre eller preline
    * en delete-knapp per ingrediens som visas i en statisk lista ovanför ingrediens fälten, gör handler i edit för att ta bort ingrediensen
    * om modelstate-problemet: 


