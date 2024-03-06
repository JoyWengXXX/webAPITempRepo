using System.Data;
using System.Linq.Expressions;

namespace CommonLibrary.Dapper.Interfaces
{
    /// <summary>
    /// Dapper建立連線
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public interface IRepository<T1, T2> where T1 : IDbContext where T2 : class
    {
        //回傳DbContext的資料庫連線
        public abstract IDbConnection CreateConnection();

        /// <summary>
        /// 取回資料清單
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Task<IEnumerable<T2>?> GetDataList(Expression<Func<T2, object>> selected, Expression<Func<T2, bool>>? predicate = null, IDbTransaction? transaction = null);

        /// <summary>
        /// 取回指定ID資料
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="predicate"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Task<T2?> GetExistedData(Expression<Func<T2, object>> selected, Expression<Func<T2, bool>>? predicate = null, IDbTransaction? transaction = null);

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="input"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Task<int> CreateData(T2 input, IDbTransaction? transaction = null);

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="input">更新資料</param>
        /// <param name="predicate">WHERE條件</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Task<int> UpdateData(Expression<Func<T2, bool>> input, Expression<Func<T2, bool>> predicate, IDbTransaction? transaction = null);

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="predicate">WHERE條件</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Task<int> DeleteData(Expression<Func<T2, bool>> predicate, IDbTransaction? transaction = null);
    }
}
