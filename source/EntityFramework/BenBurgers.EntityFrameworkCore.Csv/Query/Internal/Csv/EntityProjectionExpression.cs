/*
 * © 2022 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Exceptions;
using BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace BenBurgers.EntityFrameworkCore.Csv.Query.Internal.Csv;

/// <summary>
/// An expression that describes an entity projection.
/// </summary>
internal sealed class EntityProjectionExpression : Expression, IAccessExpression
{
    private readonly Dictionary<IProperty, IAccessExpression> propertyExpressionsMap = new();
    private readonly Dictionary<INavigation, IAccessExpression> navigationExpressionsMap = new();

    /// <summary>
    /// Initializes a new instance of <see cref="EntityProjectionExpression" />.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <param name="accessExpression">The access expression.</param>
    internal EntityProjectionExpression(IEntityType entityType, Expression accessExpression)
    {
        this.EntityType = entityType;
        this.AccessExpression = accessExpression;
        this.Name = (accessExpression as IAccessExpression)?.Name ?? entityType.Name;
    }

    /// <summary>
    /// Gets the access expression.
    /// </summary>
    internal Expression AccessExpression { get; }

    /// <summary>
    /// Gets the entity type.
    /// </summary>
    internal IEntityType EntityType { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public override ExpressionType NodeType => ExpressionType.Extension;

    /// <inheritdoc />
    public override Type Type => this.EntityType.ClrType;

    /// <summary>
    /// Binds a member.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="entityType">The entity CLR type.</param>
    /// <param name="propertyBase">The property base.</param>
    /// <returns>The expression with bound member.</returns>
    /// <exception cref="ArgumentNullException">
    /// An <see cref="ArgumentNullException" /> is thrown if the name of the member could not be evaluated.
    /// </exception>
    /// <exception cref="CsvBindingException">
    /// A <see cref="CsvBindingException" /> is thrown if a member could not be found.
    /// </exception>
    internal Expression BindMember(string name, Type entityType, out IPropertyBase propertyBase)
        => this.BindMember(MemberIdentity.Create(name), entityType, out propertyBase);

    /// <summary>
    /// Binds a member.
    /// </summary>
    /// <param name="memberInfo">The member information.</param>
    /// <param name="entityType">The entity CLR type.</param>
    /// <param name="propertyBase">The property base.</param>
    /// <returns>The expression with bound member.</returns>
    /// <exception cref="ArgumentNullException">
    /// An <see cref="ArgumentNullException" /> is thrown if the name of the member could not be evaluated.
    /// </exception>
    /// <exception cref="CsvBindingException">
    /// A <see cref="CsvBindingException" /> is thrown if a member could not be bound.
    /// </exception>
    internal Expression BindMember(MemberInfo memberInfo, Type entityType, out IPropertyBase propertyBase)
        => this.BindMember(MemberIdentity.Create(memberInfo), entityType, out propertyBase);

    private Expression BindMember(MemberIdentity memberIdentity, Type? entityClrType, out IPropertyBase propertyBase)
    {
        var entityType = this.EntityType;
        if (entityClrType is not null && !entityClrType.IsAssignableFrom(entityType.ClrType))
            entityType = entityType.GetDerivedTypes().First(e => entityClrType.IsAssignableFrom(e.ClrType));

        // Member is property?
        var property = memberIdentity.MemberInfo is null
            ? entityType.FindProperty(memberIdentity.Name ?? throw new ArgumentNullException(nameof(memberIdentity.Name)))
            : entityType.FindProperty(memberIdentity.MemberInfo);
        if (property is not null)
        {
            propertyBase = property;
            return this.BindProperty(property);
        }

        // Member is navigation?
        var navigation = memberIdentity.MemberInfo is null
            ? entityType.FindNavigation(memberIdentity.Name ?? throw new ArgumentNullException(nameof(memberIdentity.Name)))
            : entityType.FindNavigation(memberIdentity.MemberInfo);
        if (navigation is not null)
        {
            propertyBase = navigation;
            return this.BindNavigation(navigation);
        }

        throw new CsvBindingException(memberIdentity);
    }

    /// <summary>
    /// Binds a navigation.
    /// </summary>
    /// <param name="navigation">The navigation to bind.</param>
    /// <returns></returns>
    /// <exception cref="CsvBindingException"></exception>
    internal Expression BindNavigation(INavigation navigation)
    {
        if (!this.EntityType.IsAssignableFrom(navigation.DeclaringEntityType)
            && !navigation.DeclaringEntityType.IsAssignableFrom(this.EntityType))
            throw new CsvBindingException(navigation);
        if (!this.navigationExpressionsMap.TryGetValue(navigation, out var expression))
        {
            expression = navigation.IsCollection
                ? throw new NotSupportedException()
                : new EntityProjectionExpression(navigation.TargetEntityType, new ObjectAccessExpression(navigation, this.AccessExpression));
            this.navigationExpressionsMap[navigation] = expression;
        }
        return (Expression)expression;
    }

    /// <summary>
    /// Binds a property.
    /// </summary>
    /// <param name="property">The property to bind.</param>
    /// <returns>The expression with bound property.</returns>
    /// <exception cref="CsvBindingException">
    /// A <see cref="CsvBindingException" /> is thrown if the <paramref name="property" /> could not be bound.
    /// </exception>
    internal Expression BindProperty(IProperty property)
    {
        if (!this.EntityType.IsAssignableFrom(property.DeclaringEntityType)
            && !property.DeclaringEntityType.IsAssignableFrom(this.EntityType))
            throw new CsvBindingException(property);
        if (!this.propertyExpressionsMap.TryGetValue(property, out var expression))
        {
            expression = new KeyAccessExpression(property, this.AccessExpression);
            this.propertyExpressionsMap[property] = expression;
        }
        return (Expression)expression;
    }
}
