# Introduction
This is a project mean to be used for learning basic CRUD operations and Pagination using
- AspNet Web Api 2 (Not Asp Net Core, for the Asp Net Core I have another app, look links below)
- Entity Framework


I have many other implementations of this server:
- [Spring Boot + Spring Data + Jersey](https://github.com/melardev/SpringBootJerseyApiPaginatedCrud)
- [Spring Boot + Spring Data](https://github.com/melardev/SpringBootApiJpaPaginatedCrud)
- [Spring Boot Reactive + Spring Data Reactive](https://github.com/melardev/ApiCrudReactiveMongo)
- [Go with Gin Gonic](https://github.com/melardev/GoGinGonicApiPaginatedCrud)
- [Laravel](https://github.com/melardev/LaravelApiPaginatedCrud)
- [Rails + JBuilder](https://github.com/melardev/RailsJBuilderApiPaginatedCrud)
- [Rails](https://github.com/melardev/RailsApiPaginatedCrud)
- [NodeJs Express + Sequelize](https://github.com/melardev/ExpressSequelizeApiPaginatedCrud)
- [NodeJs Express + Bookshelf](https://github.com/melardev/ExpressBookshelfApiPaginatedCrud)
- [NodeJs Express + Mongoose](https://github.com/melardev/ExpressApiMongoosePaginatedCrud)
- [Python Django](https://github.com/melardev/DjangoApiCrudPaginated)
- [Python Django + Rest Framework](https://github.com/melardev/DjangoRestFrameworkPaginatedCrud)
- [Python Flask](https://github.com/melardev/FlaskApiPaginatedCrud)
- [AspNetCore](https://github.com/melardev/AspNetCoreApiPaginatedCrud)

# Understanding the project
I have controllers, services that deal with the database, Dtos that filter which thing is returned or not as response.
The controllers all return HttpResponseMessage, which is somewhat the old way, which means this code is more portable,
If for some reason you wanna return the new IHttpActionResult, it is easy, just wrap the HttpResponseMessage into ResponseMessage, the example
for the GetTodos Action in TodosController would be:

```csharp
public async Task<IHttpActionResult> GetTodos([FromUri] int page = 1, [FromUri] int pageSize = 5)
{
    var result = await _todosService.FetchMany(page, pageSize, TodoShow.All);
    return ResponseMessage(StatusCodeAndDtoWrapper.BuildSuccess(TodoListResponse.Build(result.Item2,
        Request.RequestUri.LocalPath, page,
        pageSize, result.Item1)));

}
```
More on this topic:
- [Why should I use IHttpActionResult instead of HttpResponseMessage](https://stackoverflow.com/questions/21758615/why-should-i-use-ihttpactionresult-instead-of-httpresponsemessage)
- [Action Results in Asp Net Web Api 2](https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/action-results)

I drop/create and seed database each time the app is run, this is not what we want for a more serious app, what we would want is to use migrations.
A really cool and simple guide can be found at official Microsoft docs(1min article read):
https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-3

The next come are:
- NodeJs Express + Knex
- Flask + Flask-Restful
- Laravel + Fractal
- Laravel + ApiResources
- Go with Mux
- Jersey
- Elixir

# Resources
- [Why Dtos are useful](https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5)
- [Bogus, the awesome Faker framework for .Net](https://github.com/bchavez/Bogus)
- [Create Entitis with Linq workaround](https://stackoverflow.com/questions/5325797/the-entity-cannot-be-constructed-in-a-linq-to-entities-query)
