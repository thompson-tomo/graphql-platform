# GetDefaultPage_With_Deep_SecondPage

## SQL 0

```sql
-- @value='Country1'
-- @value1='2'
-- @p='3'
SELECT b."Id", b."AlwaysNull", b."DisplayName", b."Name", b."BrandDetails_Country_Name"
FROM "Brands" AS b
WHERE b."BrandDetails_Country_Name" > @value OR (b."BrandDetails_Country_Name" = @value AND b."Id" > @value1)
ORDER BY b."BrandDetails_Country_Name", b."Id"
LIMIT @p
```

## Expression 0

```text
[Microsoft.EntityFrameworkCore.Query.EntityQueryRootExpression].OrderBy(x => x.BrandDetails.Country.Name).ThenBy(t => t.Id).Where(t => ((t.BrandDetails.Country.Name.CompareTo(value(GreenDonut.Data.Expressions.ExpressionHelpers+<>c__DisplayClass6_0`1[System.String]).value) > 0) OrElse ((t.BrandDetails.Country.Name.CompareTo(value(GreenDonut.Data.Expressions.ExpressionHelpers+<>c__DisplayClass6_0`1[System.String]).value) == 0) AndAlso (t.Id.CompareTo(value(GreenDonut.Data.Expressions.ExpressionHelpers+<>c__DisplayClass6_0`1[System.Int32]).value) > 0)))).Take(3)
```

## Result

```json
{
  "data": {
    "brandsDeep": {
      "edges": [
        {
          "cursor": "e31Db3VudHJ5MTA6MTE="
        },
        {
          "cursor": "e31Db3VudHJ5MTE6MTI="
        }
      ],
      "nodes": [
        {
          "id": 11,
          "name": "Brand:10",
          "displayName": "BrandDisplay10",
          "brandDetails": {
            "country": {
              "name": "Country10"
            }
          }
        },
        {
          "id": 12,
          "name": "Brand:11",
          "displayName": null,
          "brandDetails": {
            "country": {
              "name": "Country11"
            }
          }
        }
      ],
      "pageInfo": {
        "hasNextPage": true,
        "hasPreviousPage": true,
        "startCursor": "e31Db3VudHJ5MTA6MTE=",
        "endCursor": "e31Db3VudHJ5MTE6MTI="
      }
    }
  }
}
```

