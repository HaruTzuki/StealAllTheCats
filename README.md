# Steal all the Cats

This is a test repository about an assessment and contains an API which featches 25 Cats per call and save them to a database and there is a couple more endpoints which can get single cat or all cats from database.

## How to install
As long as there is docker file means that you can install it to your local docker container. In order to this to happen you have to open a Terminal (or CMD) and type the following command:
```bash
docker compose up --build
```

It will run the following command before actually run:
```bash
dotnet restore
dotnet tests
dotnet build
```

After running you are able to reach the api using the endpoint http://localhost:5000/swagger/

## Endpoints

### POST /api/cats/fetch
Fetches 25 cats from cat api and save them to the local database

**Request:**
> /api/cats/fetch

**Response 201:** 
```json
no-content
```

### GET /api/cats/{id}
Gets a single cat from the database. The parameter {id} could be the Id from local database or the CatId from the Cat's API Id.

**Request:**
> /api/cats/{id}

**Response 200:** 
```json
{
  "id": 0,
  "catId": "string",
  "width": 0,
  "height": 0,
  "image": "string",
  "tags": [
    "string"
  ]
}
```

**Response 404:** 
```json
{
  "type": "string",
  "title": "string",
  "status": 0,
  "detail": "string",
  "instance": "string",
  "additionalProp1": "string",
  "additionalProp2": "string",
  "additionalProp3": "string"
}
```

### GET /api/cats
Gets all available cats from the local database.

**Request:**
> 
> /api/cats/?tag={string?}&sortBy={string?}&page={integer}&pageSize={integer}
> 

* Tag: Is not mandatory, could be null per request. If it is not null or empty, it will filter the result based on this tag.
* SortBy: Is not mandatory, could be null per request. If it is not null or empty, it sorts the result based on the field. You can use `+` or `-`. The former for Ascending order and the latter for Descending order. For instance, `?sortBy=-id`
* Page: Indicates the current page. Must be greater than zero.
* PageSize: Indicates the how many items per page you want to see. Must be greater than zero and less or equal to twenty five.

*Both `Page` and `PageSize` are mandantory.


**Response 200:** 
```json
{
  "items": [
    {
      "id": 0,
      "catId": "string",
      "width": 0,
      "height": 0,
      "image": "string",
      "tags": [
        "string"
      ]
    }
  ],
  "pageSize": 0,
  "page": 0,
  "total": 0,
  "hasNextPage": true
}
```