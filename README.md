## GraphMed

One Paragraph of the project description

## Getting Started

Some notes about getting started

### Prerequisites

What things you need to install the software and how to install them

```
Examples
```

### Installing

A step by step series of examples that tell you have to get a development environment running. 
```
Example
``` 
And repeat
```
Until finished
```
End with an example of getting some data out of the system or using it for a little demo

## Documentation

Writing Cyphers to the database is done by the static class CypherHandler.cs. 
The CypherHandler can call the following functions with a ***result limit*** as a parameter: 
```
CypherHandler.Create(int? limit)
```
```
CypherHandler.Load(int? limit, int? commit)
```
```
CypherHandler.Delete(int? limit?)
```
```
CypherHandler.Drop(int? limit?)
```
```
CypherHandler.Match(int? limit?)
```
### Create

### Load
The Load-function is based on Cyphers ***LOADCSV***. Apart from the ***limit*** parameter, Load also contains the ***commit*** parameter. This means that when you call ***Load***, you need to specify how many rows in the .CSV file you want to import and the size of the periodic commit. The parameters are nullable if you don't want to set a limit or use the default commit size.
The function can in turn call on the different Nodes that you can build the graph database with: 
```
CypherHandler.Load(limit: 2000, commit: 200).Concepts(); 
```
```
CypherHandler.Load(limit: 2000, commit: 200).Descriptions(forceRelationship: true); 
```
When loading the descriptions, you have the choice of forcing a relationship on the anchoring Concept. If you do, the relationship will be made between the description and concept "on create". 
### Delete

### Drop

### Match

## Built With

* Something
* Another thing

## Contributing 

Contributions

## Version

Version info

## Authors

* **Eric Nilsson** 
* **Haris Eminovic**
* **Jakob Lindblad**
* **Niko Pavlovic**

## License

Some license info - if any

## Acknowledgements

* Hats off to 
