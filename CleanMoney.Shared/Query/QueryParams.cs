namespace CleanMoney.Shared
{
    public class QueryParams
    {
        /// <summary>
        /// Dados de paginação, quando houver. Se suprimido ou deixado em branco, a paginação padrão retorna sempre a página 1 com 10 elementos. Se o PageSize for 0, retorna todos os elementos.
        /// </summary>
        public PaginationInput? Pagination { get; set; }

        /// <summary>
        /// Dados dos filtros aplicados, se houver.
        /// </summary>
        public Filter? Filter { get; set; }

        /// <summary>
        /// Dados da ordenação, quando houver.
        /// </summary>
        public Ordering? Ordering { get; set; }

        /// <summary>
        /// Texto da caixa de pesquisa, quando houver. Cada objeto permite busca em um conjunto pré definido de campos. Consulte a documentação.
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Indica se os registros deletados/desativados devem ser retornados. Por padrão, é false.
        /// </summary>
        public bool? ShowDeleted { get; set; }

        public QueryParams() { }

        /// <summary>
        /// Cria uma instância de QueryParams com o tamanho da página = 0, que resulta na busca de todos os elementos.
        /// </summary>
        /// <returns></returns>
        public static QueryParams QueryAll() //TODO: Verificar se isso não pode ser uma constante
        {
            return new QueryParams
            {
                ShowDeleted = false,
                Pagination = new PaginationInput
                {
                    PageNumber = 1,
                    PageSize = 0
                }
            };
        }
    }
}
