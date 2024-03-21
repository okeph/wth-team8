using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RazorPagesTestSample.Data
{
    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the collection of messages in the database.
        /// </summary>
        public virtual DbSet<Message> Messages { get; set; }

        #region snippet1
        /// <summary>
        /// Retrieves all messages from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of messages.</returns>
        public async virtual Task<List<Message>> GetMessagesAsync()
        {
            return await Messages
                .OrderBy(message => message.Text)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region snippet2
        /// <summary>
        /// Adds a new message to the database asynchronously.
        /// </summary>
        /// <param name="message">The message to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async virtual Task AddMessageAsync(Message message)
        {
            await Messages.AddAsync(message);
            await SaveChangesAsync();
        }
        #endregion

        #region snippet3
        /// <summary>
        /// Deletes all messages from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async virtual Task DeleteAllMessagesAsync()
        {
            foreach (Message message in Messages)
            {
                Messages.Remove(message);
            }
            
            await SaveChangesAsync();
        }
        #endregion

        #region snippet4
        /// <summary>
        /// Deletes a message from the database asynchronously.
        /// </summary>
        /// <param name="id">The ID of the message to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async virtual Task DeleteMessageAsync(int id)
        {
            var message = await Messages.FindAsync(id);

            if (message != null)
            {
                Messages.Remove(message);
                await SaveChangesAsync();
            }
        }
        #endregion

        /// <summary>
        /// Initializes the database with a set of seeding messages.
        /// </summary>
        public void Initialize()
        {
            Messages.AddRange(GetSeedingMessages());
            SaveChanges();
        }

        /// <summary>
        /// Gets a list of seeding messages.
        /// </summary>
        /// <returns>A list of seeding messages.</returns>
        public static List<Message> GetSeedingMessages()
        {
            return new List<Message>()
            {
                new Message(){ Text = "You're standing on my scarf." },
                new Message(){ Text = "Would you like a jelly baby?" },
                new Message(){ Text = "To the rational mind, nothing is inexplicable; only unexplained." }
            };
        }
    }
}
