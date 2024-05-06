# IGS Tech Test - Scheduling API


## Objective

_(copied from [task repo](https://github.com/intelligent-growth-solutions/tech-test-software-engineer))_

Given the list of recipe names, start dates and tray numbers outlined below, create an application that queries the
Recipe API (see info below) and generates a JSON file with a schedule. The schedule should outline at exactly what time
light and water commands should be sent to the Tower, and what the light intensity or amount to water should be.

The dates in the schedule should be in UTC.

```typescript
{
    input: [
        {
            trayNumber: 1,
            recipeName: "Basil",
            startDate: "2022-01-24T12:30:00.0000000Z"
        },
        {
            trayNumber: 2,
            recipeName: "Strawberries",
            startDate: "2021-13-08T17:33:00.0000000Z"
        },
        {
            trayNumber: 3,
            recipeName: "Basil",
            startDate: "2030-01-01T23:45:00.0000000Z"
        }
    ]
}
```

## How to run the code

1. Clone the [task repo](https://github.com/intelligent-growth-solutions/tech-test-software-engineer)
2. `cd` into the cloned task repo and run `docker-compose up`
3. You'll now have the RecipeAPI running on [http:localhost:8080](http:localhost:8080)
4. Clone **this** repo
5. `cd` into the cloned repo and run `docker-compose up`
6. You'll now have the SchedulingAPI running on [http:localhost:8090](http:localhost:8090)
7. Navigate to [http:localhost:8090/swagger](http:localhost:8090/swagger)
8. You'll see three endpoints, all of them prepopulated with sensible example data, so you can run them directly from SwaggerUI.
    1. GET `api/v1/schedule`
        * a simple "ping" endpoint to check if the API is up
    2. POST `api/v1/schedule/multiple`
        * accepts a list of inputs as specified in section [Objective](#objective)
    3. POST `api/v1/schedule/single`
        * accepts a single input using the schema of a single item in the input list in section [Objective](#objective)
9. Use the _Try it out_ button on the `api/v1/schedule/multiple` to run the example input from
   section [Objective](#objective)

## Notes on my code 

### Initial Thoughts

* Given that the data is passed in as JSON and JSON should be returned, creating an API seems sensible
    * A simple WebAPI endpoint processing the input JSON and returning the resulting JSON
    * I could possibly add an extra feature where I can process a single input/not just a list


### Code comments 
I added `REMARK:` code comments throughout the whole codebase. 
These comments document things like: 
* Why did I make a certain design decision?
* Are there alternatives to this approach?
* What would I do differently to make this code ready for production

In addition, the [following section](#design-decisions) outlines some design decisions that I wanted to highlight. 

### Design decisions
1. I decided to use API-Controllers instead of minimal API
    * For this simple example a minimal API would have been sufficient
    * However, I like having the code separate
    * Also, in case you wanted to extend this API, it's nice to have separate controllers for different routes


2. I decided to use composition instead of inheritance
    * I prefer having all logic/properties defined in one class instead of having to jump up the inheritance chain to
      find the properties.
    * However, I'm always happy to convinced otherwise. One of my fav concepts is 'Disagree and Commit' - let's discuss
      it and then decide what's best for the situation.


3. I decided to use one command type for both watering and lighting commands
    * Unused fields will just be null
    * Could make a case for using separate forms to send less data over the wire
    * Having a single model allows for easy and fast development for now. I would revisit this for produdction code


4. I decided against using a custom JSON serializer and handle invalid dates by using a custom setter 
    * In hindsight, I would have chosen a custom JSON serializer 
    * Initially, I thought a custom JSON serializer would be overkill and found my solution with a custom setter quite neat
    * However, after working on this project for a bit, I'd say using a custom JSON serializer would be better because
      * it's a common standard 
      * if an error occurs, it's obvious that it happened during the parsing stage 
      * it allows returning an error early and not storing data that's invalid 
    * My current approach could also return early, but I feel like it would be less clean
    * Another downside of the current code, it does not return an error message stating that a date was invalid (see section [Outstanding Issues](#outstanding-issues)), it simply logs the error and
      * skips the request (if it's part of a list) 
      * returns a 500 (if it's the only request/all requests are invalid)


5. I'm cautious when it comes to sensitive data - e.g. `appsettings.Development.json`
   - I added `appsettings.Development.json` to .gitignore
       - Depending on the dev setup this file could contains sensitive information. I'd opt for not commiting it to source control.
           - for production: defo use secret management
           - for dev: ideally use secret managment. at least keep settings local and NEVER commit sensitive configs
   - I added `appsettings.Development.json` to .dockerignore 
     - currently without effect, see section [Outstanding Issues](#outstanding-issues)

These are just a handful of decisions. Obviously coding includes almost infinite choice (I think that's part of the fun).
I'm happy to answer any other questions during the next interview.

### Outstanding Issues
- `**/appsettings.Development.json` in my .dockerignore doesn't work
    - I'm not sure why and would debug this further before moving to production
    - Including files that shouldn't be included in your docker-image poses a security risk
        - I'd advise against using the `COPY . .` command and explicitly state files you want to copy for a production app
- API doesn't return a message to state that dates are invalid 

### Changes required for making this code production-ready
The main issue is lack of feedback. All the code you see was solely written by me without discussing anything with other engineers or product.
This approach can easily lead to a final product that doesn't meet the requirements.

That put aside, here are a few improvements on code level: 
- enable HTTPS
- add proper monitoring 
- add more unit tests
  - for JSON parsing 
  - for other parts of the scheduling logic
- factor out the HttpService and add caching

## Final thoughts 
I enjoyed this little challenge. It got me thinking about different parts of the development lifecycle and gave me the opportunity to practice some skills I haven't used in a while (like Docker). 

