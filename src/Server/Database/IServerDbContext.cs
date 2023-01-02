using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Server.Database;

public interface IServerDbContext
{
	DbContextId ContextId { get; }
	IModel Model { get; }
	ChangeTracker ChangeTracker { get; }
	DatabaseFacade Database { get; }
	event EventHandler<SavingChangesEventArgs>? SavingChanges;
	event EventHandler<SavedChangesEventArgs>? SavedChanges;
	event EventHandler<SaveChangesFailedEventArgs>? SaveChangesFailed;
	EntityEntry Add(object entity);
	EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
	ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
	ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
	void AddRange(IEnumerable<object> entities);
	void AddRange(params object[] entities);
	Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
	Task AddRangeAsync(params object[] entities);
	EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
	EntityEntry Attach(object entity);
	void AttachRange(params object[] entities);
	void AttachRange(IEnumerable<object> entities);
	void Dispose();
	ValueTask DisposeAsync();
	EntityEntry Entry(object entity);
	EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
	bool Equals(object? obj);
	TEntity? Find<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(params object?[]? keyValues) where TEntity : class;
	object? Find([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, params object?[]? keyValues);
	ValueTask<TEntity?> FindAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(object?[]? keyValues, CancellationToken cancellationToken) where TEntity : class;
	ValueTask<object?> FindAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, params object?[]? keyValues);
	ValueTask<object?> FindAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, object?[]? keyValues, CancellationToken cancellationToken);
	ValueTask<TEntity?> FindAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(params object?[]? keyValues) where TEntity : class;
	IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression);
	int GetHashCode();
	EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
	EntityEntry Remove(object entity);
	void RemoveRange(params object[] entities);
	void RemoveRange(IEnumerable<object> entities);
	int SaveChanges();
	int SaveChanges(bool acceptAllChangesOnSuccess);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
	DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(string name) where TEntity : class;
	DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>() where TEntity : class;
	string? ToString();
	EntityEntry Update(object entity);
	EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
	void UpdateRange(params object[] entities);
	void UpdateRange(IEnumerable<object> entities);
}
