namespace CleanMoney.Shared
{
    public class PaginationInput
    {
        /// <summary>
        /// Número da página a ser retornada. Padrão: 1
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Quantidade de itens por página. Padrão: 10
        /// Se for passado 0, retorna todos os itens
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
