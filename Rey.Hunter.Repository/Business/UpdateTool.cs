using System;
using Rey.Hunter.Models2;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace Rey.Hunter.Repository.Business {
    public class UpdateTool<TModel> : IDisposable
        where TModel : class, IModel {
        private TModel Model { get; }
        private IRepository<TModel> Repository { get; }
        private IRepositoryManager Manager => this.Repository.Manager;
        private bool Updated { get; set; }

        public UpdateTool(IRepository<TModel> repository, TModel model) {
            this.Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public void Update<T>(Expression<Func<TModel, IModelRef<T>>> expr)
            where T : class, IModel {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));

            var modelRef = GetValue(this.Model, expr);
            this.Update(modelRef);
        }

        public void Update<T, T2>(Expression<Func<TModel, T2>> expr1, Expression<Func<T2, IModelRef<T>>> expr2)
            where T : class, IModel {
            if (expr1 == null)
                throw new ArgumentNullException(nameof(expr1));

            if (expr2 == null)
                throw new ArgumentNullException(nameof(expr2));

            var middle = GetValue(this.Model, expr1);
            var modelRef = GetValue(middle, expr2);
            this.Update(modelRef);
        }

        public void Update<T>(Expression<Func<TModel, IEnumerable<IModelRef<T>>>> expr)
            where T : class, IModel {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));

            var modelRefs = GetValue(this.Model, expr);
            this.Update(modelRefs);
        }

        public void Update<T, T2>(Expression<Func<TModel, IEnumerable<T2>>> expr1, Expression<Func<T2, IModelRef<T>>> expr2)
            where T : class, IModel {
            if (expr1 == null)
                throw new ArgumentNullException(nameof(expr1));

            if (expr2 == null)
                throw new ArgumentNullException(nameof(expr2));

            var middle = GetValue(this.Model, expr1);
            foreach (var item in middle) {
                var modelRef = GetValue(item, expr2);
                this.Update(modelRef);
            }
        }

        public void Dispose() {
            if (this.Updated) {
                this.Repository.ReplaceOne(this.Model);
                this.Updated = false;
            }
        }

        private void Update<T>(IModelRef<T> modelRef, T model)
            where T : class, IModel {
            if (NeedUpdate(model, modelRef)) {
                modelRef.Update(model);
                if (!this.Updated) { this.Updated = true; }
            }
        }

        private void Update<T>(IModelRef<T> modelRef)
            where T : class, IModel {
            this.Update(modelRef, this.Manager.Repository<T>().FindOne(modelRef.Id));
        }

        private void Update<T>(IEnumerable<IModelRef<T>> modelRefs)
            where T : class, IModel {
            foreach (var modelRef in modelRefs) {
                this.Update(modelRef);
            }
        }

        private static bool NeedUpdate(IModel model, IModelRef modelRef) {
            if (model.ModifyAt == null)
                return false;

            if (modelRef.UpdateAt == null
                || model.ModifyAt > modelRef.UpdateAt)
                return true;

            return false;
        }

        private static MemberExpression GetMember<T, TMember>(Expression<Func<T, TMember>> expr) {
            var nodeType = expr.Body.NodeType;
            if (nodeType == ExpressionType.Convert || nodeType == ExpressionType.ConvertChecked) {
                return (expr.Body as UnaryExpression)?.Operand as MemberExpression;
            }
            return expr.Body as MemberExpression;
        }

        private static Stack<MemberExpression> GetMemberStack<T, TMember>(Expression<Func<T, TMember>> expr) {
            var member = GetMember(expr);
            var stack = new Stack<MemberExpression>();
            while (member != null) {
                stack.Push(member);
                member = member.Expression as MemberExpression;
            }
            return stack;
        }

        private static IEnumerable<PropertyInfo> GetPropertyChain<T, TMember>(Expression<Func<T, TMember>> expr) {
            var stack = GetMemberStack(expr);
            var type = typeof(T);
            var chain = new List<PropertyInfo>();

            while (stack.Count > 0) {
                var member = stack.Pop();
                var name = member.Member.Name;
                var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                chain.Add(prop);

                type = member.Type;
            }

            return chain;
        }

        private static TMember GetValue<T, TMember>(T model, Expression<Func<T, TMember>> expr) {
            var chain = GetPropertyChain(expr);
            object value = model;
            foreach (var prop in chain) {
                value = prop.GetValue(value);
            }
            return (TMember)value;
        }
    }
}
