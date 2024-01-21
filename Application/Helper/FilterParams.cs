namespace Application.Helper;
public class FilterParams
{
	private int _pageSize = 10;
	private const int MaxPagesSize = 50;
	public int PageNumber { get; set; } = 1;
	public List<string>? FilterOptions { get; set; }
	internal List<FilterOptions>? _filterOptions;
	public List<FilterOptions>? FilterOptions2
	{
		get { return _filterOptions = Filter(FilterOptions); }
		set { _filterOptions = value; }
	}
	public int PageSize
	{
		get => _pageSize;
		set => _pageSize = (value > MaxPagesSize) ? MaxPagesSize : value;
	}

	public List<FilterOptions>? Filter(List<string>? FilterOptions)
	{
		if (FilterOptions != null)
		{
			_filterOptions = new List<FilterOptions> { };
			foreach (var word in FilterOptions)
			{
				string[] words = word.Split('-');
				_filterOptions.Add(new FilterOptions { field = words[0], operators = words[1], value = words[2] });
			}
			return _filterOptions;
		}
		return null;
	}
}
