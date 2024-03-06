using CommonLibrary.Dapper.Interfaces;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CommonLibrary.Dapper
{
    public class Repository<T1, T2> : IRepository<T1, T2> where T1 : IDbContext where T2 : class
    {
        private readonly IDbConnection _dbConnection;

        public Repository(T1 dbContext)
        {
            _dbConnection = dbContext.CreateConnection();
        }

        public IDbConnection CreateConnection()
        {
            return _dbConnection;
        }

        public async Task<int> CreateData(T2 input, IDbTransaction? transaction = null)
        {
            var TableAttribute = typeof(T2).GetCustomAttribute<TableAttribute>();  // 取得資料表名稱
            var Properties = typeof(T2).GetProperties();  // 取得類別的所有屬性

            // 取得識別欄位名稱
            var IdentityColumnName = Properties
                .FirstOrDefault(p => p.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                ?.Name;

            StringBuilder SQL = new StringBuilder();
            SQL.Append($"INSERT INTO [{TableAttribute.Name}] (");
            foreach (var prop in Properties)
            {
                if (prop.Name != IdentityColumnName)  // 排除識別欄位
                {
                    SQL.Append($"[{prop.Name}],");
                }
            }
            SQL.Remove(SQL.Length - 1, 1);
            if (IdentityColumnName != null)
                SQL.Append($") OUTPUT INSERTED.{IdentityColumnName} VALUES (");
            else
                SQL.Append(") VALUES (");
            foreach (var prop in Properties)
            {
                if (prop.Name != IdentityColumnName)  // 排除識別欄位
                {
                    SQL.Append($"@{prop.Name},");
                }
            }
            SQL.Remove(SQL.Length - 1, 1);
            SQL.Append(")");
            string SQLString = SQL.ToString();
            return await _dbConnection.ExecuteScalarAsync<int>(SQLString, input, transaction: transaction);
        }

        public async Task<int> UpdateData(Expression<Func<T2, bool>> input, Expression<Func<T2, bool>> predicate, IDbTransaction? transaction = null)
        {
            var TableAttribute = typeof(T2).GetCustomAttribute<TableAttribute>();  // 取得資料表名稱
            StringBuilder SQL = new StringBuilder();
            SQL.Append($"UPDATE [{TableAttribute.Name}]");
            SQL.Append(" SET ");

            // 使用 SqlExpressionVisitor 來處理 input LINQ 表達式，獲取需要更新的欄位和值
            var inputVisitor = new UpdateExpressionVisitor();
            inputVisitor.Visit(input);
            SQL.Append(inputVisitor.Sql);

            var visitor = new WhereExpressionVisitor();
            visitor.Visit(predicate);
            SQL.Append(" WHERE ");
            SQL.Append(visitor.Sql);

            // 創建兩個不同的參數集合
            var inputParameters = inputVisitor.Parameters;
            var whereParameters = visitor.Parameters;

            // 將這兩個參數集合合併到一個 DynamicParameters 對象中
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(inputParameters);
            foreach (var item in whereParameters.ParameterNames)
            {
                if (!inputParameters.ParameterNames.Contains(item))
                    parameters.Add(item, whereParameters.Get<object>(item));
            }
            string SQLString = SQL.ToString();
            return await _dbConnection.ExecuteAsync(SQLString, parameters, transaction: transaction);
        }

        public async Task<int> DeleteData(Expression<Func<T2, bool>> predicate, IDbTransaction? transaction = null)
        {
            var TableAttribute = typeof(T2).GetCustomAttribute<TableAttribute>();  // 取得資料表名稱
            StringBuilder SQL = new StringBuilder();
            SQL.Append($"DELETE FROM [{TableAttribute.Name}] ");
            if (predicate != null)
            {
                var visitor = new WhereExpressionVisitor();
                visitor.Visit(predicate);
                SQL.Append("WHERE ");
                SQL.Append(visitor.Sql);// 在這裡將 Parameters 添加到 Dapper 執行中
                var parameters = visitor.Parameters;
                string SQLString = SQL.ToString();
                return await _dbConnection.ExecuteAsync(SQLString, parameters, transaction: transaction);
            }
            else
            {
                string SQLString = SQL.ToString();
                return await _dbConnection.ExecuteAsync(SQLString, transaction: transaction);
            }
        }

        public async Task<IEnumerable<T2>?> GetDataList(Expression<Func<T2, object>> selected, Expression<Func<T2, bool>>? predicate = null, IDbTransaction? transaction = null)
        {
            var TableAttribute = typeof(T2).GetCustomAttribute<TableAttribute>();  // 取得資料表名稱
            StringBuilder SQL = new StringBuilder();

            // 使用 SqlExpressionVisitor 來處理 input LINQ 表達式，獲取需要更新的欄位和值
            var inputVisitor = new SelectExpressionVisitor();
            inputVisitor.Visit(selected);
            inputVisitor.BuildSelectClause();
            SQL.Append($"SELECT {inputVisitor.Sql} ");

            SQL.Append($"FROM [{TableAttribute.Name}] ");

            var visitor = new WhereExpressionVisitor();
            visitor.Visit(predicate);
            if (predicate != null && !string.IsNullOrEmpty(visitor.Sql))
                SQL.Append("WHERE ");
            SQL.Append(visitor.Sql);

            // 創建兩個不同的參數集合
            var inputParameters = inputVisitor.Parameters;
            var whereParameters = visitor.Parameters;

            // 將這兩個參數集合合併到一個 DynamicParameters 對象中
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(inputParameters);
            foreach (var item in whereParameters.ParameterNames)
            {
                if (!inputParameters.ParameterNames.Contains(item))
                    parameters.Add(item, whereParameters.Get<object>(item));
            }
            string SQLString = SQL.ToString();
            return await _dbConnection.QueryAsync<T2>(SQLString, parameters, transaction: transaction);
        }

        public async Task<T2?> GetExistedData(Expression<Func<T2, object>> selected, Expression<Func<T2, bool>>? predicate = null, IDbTransaction? transaction = null)
        {
            var TableAttribute = typeof(T2).GetCustomAttribute<TableAttribute>();  // 取得資料表名稱
            StringBuilder SQL = new StringBuilder();

            // 使用 SqlExpressionVisitor 來處理 input LINQ 表達式，獲取需要更新的欄位和值
            var inputVisitor = new SelectExpressionVisitor();
            inputVisitor.Visit(selected);
            inputVisitor.BuildSelectClause();
            SQL.Append($"SELECT {inputVisitor.Sql} ");

            SQL.Append($"FROM [{TableAttribute.Name}] ");

            var visitor = new WhereExpressionVisitor();
            visitor.Visit(predicate);
            if (predicate != null)
                SQL.Append("WHERE ");
            SQL.Append(visitor.Sql);

            // 創建兩個不同的參數集合
            var inputParameters = inputVisitor.Parameters;
            var whereParameters = visitor.Parameters;

            // 將這兩個參數集合合併到一個 DynamicParameters 對象中
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(inputParameters);
            foreach (var item in whereParameters.ParameterNames)
            {
                if (!inputParameters.ParameterNames.Contains(item))
                    parameters.Add(item, whereParameters.Get<object>(item));
            }
            string SQLString = SQL.ToString();
            return await _dbConnection.QueryFirstOrDefaultAsync<T2>(SQLString, parameters, transaction: transaction);
        }

        /// <summary>
        /// 組合SELECT欄位的語法
        /// </summary>
        private class SelectExpressionVisitor : ExpressionVisitor
        {
            private readonly DynamicParameters _parameters = new DynamicParameters();
            private List<string> _selectedColumns = new List<string>();

            public string Sql { get; private set; } = string.Empty;
            public DynamicParameters Parameters => _parameters;

            protected override Expression VisitMember(MemberExpression node)
            {
                var columnName = node.Member.Name;
                _selectedColumns.Add(columnName);

                return base.VisitMember(node);
            }

            public void BuildSelectClause()
            {
                Sql = string.Join(", ", _selectedColumns);
            }
        }
        /// <summary>
        /// 組合UPDATE欄位的語法
        /// </summary>
        private class UpdateExpressionVisitor : ExpressionVisitor
        {
            private readonly DynamicParameters _parameters = new DynamicParameters();

            public string Sql { get; private set; } = string.Empty;
            public DynamicParameters Parameters => _parameters;

            protected override Expression VisitBinary(BinaryExpression node)
            {
                if (node.NodeType == ExpressionType.Equal)
                {
                    if (node.Left is MemberExpression left)
                    {
                        var leftMemberName = left.Member.Name;
                        var rightValue = Expression.Lambda(node.Right).Compile().DynamicInvoke(); //取得right.Right的值
                        var paramName = $"@{left.Member.Name}";
                        _parameters.Add(paramName, rightValue); // 將常數值作為 Dapper 參數添加

                        Sql += $"{leftMemberName} = {paramName}";
                    }
                    else if (node.Left is ConstantExpression constant)
                    {
                        var leftMemberName = constant.Value.ToString();
                        var rightValue = Expression.Lambda(node.Right).Compile().DynamicInvoke(); //取得right.Right的值
                        var paramName = $"@{constant.Value}";
                        _parameters.Add(paramName, rightValue); // 將常數值作為 Dapper 參數添加

                        Sql += $"{leftMemberName} = {paramName}";
                    }
                    else if (node.Left is UnaryExpression unary)
                    {
                        var leftMemberName = ((MemberExpression)unary.Operand).Member.Name;
                        var rightValue = Expression.Lambda(node.Right).Compile().DynamicInvoke(); //取得right.Right的值
                        var paramName = $"@{leftMemberName}";
                        _parameters.Add(paramName, rightValue); // 將常數值作為 Dapper 參數添加

                        Sql += $"{leftMemberName} = {paramName}";
                    }
                }
                else if (node.NodeType == ExpressionType.AndAlso)
                {
                    Visit(node.Left);
                    Sql += " , ";
                    Visit(node.Right);
                }
                return node;
            }
        }
        /// <summary>
        /// 組合WHERE的語法
        /// </summary>
        private class WhereExpressionVisitor : ExpressionVisitor
        {
            private readonly DynamicParameters _parameters = new DynamicParameters();

            public string Sql { get; private set; } = string.Empty;
            public int parameterCount { get; private set; } = 1;
            public DynamicParameters Parameters => _parameters;

            /// <summary>
            /// 針對Expression中集合的Contains做處理
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.NodeType == ExpressionType.MemberAccess)
                {
                    var paramName = $"@{node.Member.Name}{parameterCount}";
                    var rightValue = GetValue(node);
                    if (rightValue != null)
                    {
                        _parameters.Add(paramName, rightValue); // 將常數值作為 Dapper 參數添加
                        Sql += $"{node.Member.Name} IN {paramName}";
                    }
                }
                parameterCount++;
                return node;
            }

            /// <summary>
            /// 處理其於的Expression狀況
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitBinary(BinaryExpression node)
            {
                if (node.NodeType == ExpressionType.AndAlso)
                {
                    Visit(node.Left);
                    Sql += " AND ";
                    Visit(node.Right);
                }
                else if (node.NodeType == ExpressionType.OrElse)
                {
                    Visit(node.Left);
                    Sql += " OR ";
                    Visit(node.Right);
                }
                else
                {
                    if (node.Left is MemberExpression left)
                    {
                        var leftMemberName = left.Member.Name;
                        var paramName = $"@{left.Member.Name}{parameterCount}";
                        var rightValue = Expression.Lambda(node.Right).Compile().DynamicInvoke(); //取得right.Right的值
                        _parameters.Add(paramName, rightValue); // 將常數值作為 Dapper 參數添加

                        if (node.NodeType == ExpressionType.Equal)
                            Sql += $"{leftMemberName} = {paramName}";
                        if (node.NodeType == ExpressionType.NotEqual)
                            Sql += $"{leftMemberName} <> {paramName}";
                        if (node.NodeType == ExpressionType.GreaterThan)
                            Sql += $"{leftMemberName} > {paramName}";
                        if (node.NodeType == ExpressionType.GreaterThanOrEqual)
                            Sql += $"{leftMemberName} >= {paramName}";
                        if (node.NodeType == ExpressionType.LessThan)
                            Sql += $"{leftMemberName} < {paramName}";
                        if (node.NodeType == ExpressionType.LessThanOrEqual)
                            Sql += $"{leftMemberName} <= {paramName}";
                        if (node.NodeType == ExpressionType.Call)
                            Sql += $"{leftMemberName} IN {paramName}";
                    }
                    else if (node.Left is ConstantExpression constant)
                    {
                        var leftMemberName = constant.Value.ToString();
                        var rightValue = Expression.Lambda(node.Right).Compile().DynamicInvoke(); //取得right.Right的值
                        var paramName = $"@{constant.Value}";
                        _parameters.Add(paramName, rightValue); // 將常數值作為 Dapper 參數添加

                        Sql += $"{leftMemberName} = {paramName}";
                    }
                    else if (node.Left is UnaryExpression unary)
                    {
                        var leftMemberName = ((MemberExpression)unary.Operand).Member.Name;
                        var rightValue = Expression.Lambda(node.Right).Compile().DynamicInvoke(); //取得right.Right的值
                        var paramName = $"@{leftMemberName}";
                        _parameters.Add(paramName, rightValue); // 將常數值作為 Dapper 參數添加

                        Sql += $"{leftMemberName} = {paramName}";
                    }
                }
                parameterCount++;
                return node;
            }

            private object GetValue(MemberExpression member)
            {
                try
                {
                    var objectMember = Expression.Convert(member, typeof(object));

                    var getterLambda = Expression.Lambda<Func<object>>(objectMember);

                    var getter = getterLambda.Compile();

                    return getter();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}