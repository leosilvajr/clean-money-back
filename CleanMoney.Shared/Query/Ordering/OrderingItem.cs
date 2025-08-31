namespace CleanMoney.Shared
{
    public class OrderingItem
    {
        /// <summary>
        /// Nome do campo a ser ordenado
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Direção da ordenação. Deve ser Asc ou Desc. Padrão: Asc
        /// </summary>
        public SortDirection Direction { get; set; } = SortDirection.Asc;
    }
}
