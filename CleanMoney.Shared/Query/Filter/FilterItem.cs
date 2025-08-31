namespace CleanMoney.Shared
{
    public class FilterItem
    {
        /// <summary>
        /// Nome do campo
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Operador do filtro. Um dos seguintes: Equals, Contains, StartsWith, EndsWith, GreaterThan, LessThan
        /// </summary>
        public FilterOperator Operator { get; set; } = FilterOperator.Equals;

        /// <summary>
        /// Valor digitado no campo de filtro
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
