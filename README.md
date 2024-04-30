# IGS Tech Test - Scheduling API

## Objective (copied from [task repo](https://github.com/intelligent-growth-solutions/tech-test-software-engineer)

Given the list of recipe names, start dates and tray numbers outlined below, create an application that queries the Recipe API (see info below) and generates a JSON file with a schedule. The schedule should outline at exactly what time light and water commands should be sent to the Tower, and what the light intensity or amount to water should be.

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

## Ideas
- Given that the data is passed in as JSON and JSON should be returned, creating an API seems sensible
- a simple WebAPI endpoint processing the input JSON and returning the resulting JSON should work

## Steps taken 
- create empty git repo, add README and .gitignore
- create template WebAPI project using Rider 
