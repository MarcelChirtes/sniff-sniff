using Marcel.DbModels.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marcel.Access
{
    public interface IDishAccess
    {
        Dish Find(string url);

        void Insert(Dish dish);

        void Update(Dish dish);

        IEnumerable<Dish> GetAll();

        IEnumerable<Dish> GetAll(int skip, int take);
    }

    public class DishAccess : IDishAccess
    {
        private readonly ILogger<DishAccess> logger;
        private readonly MyDbContext dbContext;

        public DishAccess(ILogger<DishAccess> logger,
            MyDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Dish Find(string url)
        {
            return dbContext.Dish.FirstOrDefault(x => x.Url == url);
        }

        public void Insert(Dish dish)
        {
            dbContext.Add(dish);
            dbContext.SaveChanges();
        }

        public void Update(Dish dish)
        {
            var dbDish = dbContext.Dish.FirstOrDefault(x => x.Url == dish.Url);
            dbDish.MenuTitle = dish.MenuTitle;
            dbDish.MenuDescription = dish.MenuDescription;
            dbDish.MenuSectionTitle = dish.MenuSectionTitle;
            dbDish.DishName = dish.DishName;
            dbDish.DishDescription = dish.DishDescription;
            dbContext.SaveChanges();
        }

        public IEnumerable<Dish> GetAll()
        {
            return dbContext.Dish;
        }

        public IEnumerable<Dish> GetAll(int skip, int take)
        {
            return dbContext.Dish.Skip(skip).Take(take);
        }
    }
}