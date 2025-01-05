using System.ComponentModel.DataAnnotations;

namespace MinimalAPIsMovies.Entities
{
    public class Genre
    {
        /// <summary>
        /// Entity Framework infers that the purpose of this property is a primary key.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The null! syntax in C# is known as the null-forgiving operator. It is used to inform the compiler that 
        /// you are intentionally assigning a null value to a non-nullable reference type property or variable. 
        /// This suppresses the compiler's nullability warnings.
        /// </summary>
        //[StringLength(150)] this is an example of a data annotations, it is used to set the maximum length of the string, 
        // but it is not used in this example since we are using the fluent API in the ApplicationDbContext class, you
        // would need this for validation purposes.

        public string Name { get; set; } = null!;
    }
}

