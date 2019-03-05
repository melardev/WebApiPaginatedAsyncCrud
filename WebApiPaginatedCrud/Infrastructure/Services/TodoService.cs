using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebApiPaginatedCrud.Data;
using WebApiPaginatedCrud.Entities;
using WebApiPaginatedCrud.Enums;

namespace WebApiPaginatedCrud.Infrastructure.Services
{
    public class TodoService
    {
        public TodoService()
        {
        }

        public async Task<Tuple<int, List<Todo>>> FetchMany(int page = 1, int pageSize = 5,
            TodoShow show = TodoShow.All)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                /*  .Select(t => new Todo
                       {
                           Id = t.Id,
                           Title = t.Title,
                           Description = t.Description,
                           Completed = t.Completed,
                           CreatedAt = t.CreatedAt,
                           UpdatedAt = t.UpdatedAt
                       })
                   */

                // Retrieve hwo many articles with our criteria(All, Completed or Pending)
                int offset = (page - 1) * pageSize;
                IQueryable<Todo> queryable = dbContext.Todos.OrderBy(t => t.CreatedAt);

                if (show == TodoShow.Completed)
                {
                    queryable = queryable.Where(t => t.Completed);
                }
                else if (show == TodoShow.Pending)
                {
                    queryable = queryable.Where(t => !t.Completed);
                }

                int totalCount;
                List<Todo> todos;
                if (show != TodoShow.All)
                {
                    // https://stackoverflow.com/questions/5325797/the-entity-cannot-be-constructed-in-a-linq-to-entities-query
                    // for complete/pending
                    totalCount = await queryable.CountAsync();
                    todos = (await queryable.Skip(offset)
                            .Take(pageSize)
                            .Select(t => new // let's create an anonymous type
                            {
                                // t.Id is the same as Id = t.Id
                                t.Id,
                                t.Title,
                                t.Description,
                                t.Completed,
                                t.CreatedAt,
                                t.UpdatedAt
                            })
                            .ToListAsync())
                        .Select(anonymousType => new Todo
                        {
                            Id = anonymousType.Id,
                            Title = anonymousType.Title,
                            Description = anonymousType.Description,
                            Completed = anonymousType.Completed,
                            CreatedAt = anonymousType.CreatedAt,
                            UpdatedAt = anonymousType.UpdatedAt
                        })
                        .ToList();
                }
                else
                {
                    // for show all
                    totalCount = await dbContext.Todos.CountAsync();
                    todos = await queryable.Skip(offset).Take(pageSize).ToListAsync();
                }


                return Tuple.Create(totalCount, todos);
            }
        }


        public async Task<Todo> Get(int todoId)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                return await dbContext.Todos.FirstOrDefaultAsync(t => t.Id == todoId);
            }
        }

        public async Task<Todo> CreateTodo(Todo todo)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                dbContext.Todos.Add(todo);
                await dbContext.SaveChangesAsync();
                return todo;
            }
        }

        public async Task<Todo> Update(int id, Todo todoFromUserInput)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                Todo todoFromDb = dbContext.Todos.First(t => t.Id == id);
                todoFromDb.Title = todoFromUserInput.Title;
                todoFromDb.Description = todoFromUserInput.Description;
                todoFromDb.Completed = todoFromUserInput.Completed;

                // Not needed, it is set in ApplicationDbContext
                dbContext.Entry(todoFromDb).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return todoFromDb;
            }
        }


        public async Task Delete(int todoId)
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                Todo todoFromDb = await dbContext.Todos.FirstAsync(t => t.Id == todoId);
                dbContext.Todos.Remove(todoFromDb);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAll()
        {
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                dbContext.Todos.RemoveRange(dbContext.Todos);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}