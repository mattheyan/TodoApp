using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ExoModel.EntityFramework;
using ExoModel;
using ExoRule;
using ExoRule.DataAnnotations;

namespace TodoApp.Models
{
	public class TodoContext : DbContext
	{
		static IRuleProvider annotationRules = new AnnotationsRuleProvider(typeof(List).Assembly);

		public DbSet<List> Lists { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Priority> Priorities { get; set; }
		public DbSet<ListItem> ListItems { get; set; }

		public static TodoContext Current
		{
			get
			{
				return (TodoContext)((EntityFrameworkModelTypeProvider.EntityModelType)ModelContext.Current.GetModelType<List>()).GetObjectContext();
			}
		}

		public TodoContext()
			: base("TodoContext")
		{
			Configuration.LazyLoadingEnabled = true;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}
