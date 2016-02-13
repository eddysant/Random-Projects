var nextPage = 0;
var previousPage = 0;
var previousId = -1;
var previousColour = '';

function lcboSetup()
{
	$('#prev').click(GetPrevious);
	$('#next').click(GetNext);	

	$('#promotionCheck').click(NewSearch);
	$('#offerCheck').click(NewSearch);
	$('#airmilesCheck').click(NewSearch);

	$('#prev').prop("disabled", true);
	$('#next').prop("disabled", true);
	$('#offerCheck').prop('checked', 'checked');	
							
	HideLeftPane();
	HideRightPane();
}

function NewSearch()
{
	GetProducts(1);
}
			
function GetProducts(page)
{												
	$('#results').empty();
	
	$.ajax({ 
		type: 'GET',
		url: GenerateProductsUrl(page),
		dataType: 'jsonp',
		success: GetProductsCallBack,
		error: ErrorMessage
	});						
}

function GenerateProductsUrl(page)
{
	if (isNaN(page))	
		page = 1;
		
	var url = 'http://lcboapi.com/products?page=' + page + '&where_not=is_discontinued';
				
	if ($('#promotionCheck').is(':checked'))
		url += '&where=has_value_added_promotion';
	else if ($('#offerCheck').is(':checked'))
		url += '&where=has_limited_time_offer';
	else if ($('#airmilesCheck').is(':checked'))
		url += '&where=has_bonus_reward_miles';
					
	url += '&callback=?';
	
	return url;
}

function GenerateIndividualProductUrl(productId)
{
	return 'http://lcboapi.com/products/' + productId;
}

function GetIndividualProduct(productId)
{	
	if ($('#product' +  previousId).length)
		$('#product' +  previousId).css("background-color", previousColour);
				
	previousId = productId;
	previousColour =  $('#product' +  productId).css("background-color");
					
	$('#product' +  productId).css("background-color","#993399");
	
	$.ajax({ 
		type: 'GET',
		url: GenerateIndividualProductUrl(productId),
		dataType: 'jsonp',
		success: GetIndividualProductCallBack,
		error: ErrorMessage					
	});										
}

function ErrorMessage(data)
{
	if (data.status != 200)
	{
		$('#error').empty();
		$('#error').append(data.message);
	}
}

function GetIndividualProductCallBack(data)
{				
	$('#singleProductTitle').empty();
	$('#productDiv').empty();

	$('#singleProductTitle').append(data.result.name);			
	
	if (data.result.image_thumb_url != null)				
		$('#productDiv').append('<span style="float:right"><img src="' + data.result.image_thumb_url + '" /></span>');	
	if (data.result.primary_category != null)
		$('#productDiv').append('<div><b>Category</b>: ' + data.result.primary_category + '</div>');			
	if (data.result.price_in_cents != null)					
		$('#productDiv').append('<div><b>Price</b>: $' + data.result.price_in_cents/100 + '</div>');				
	if (data.result.regular_price_in_cents != null)
		$('#productDiv').append('<div><b>Regular price</b>: $' + data.result.regular_price_in_cents/100 + '</div>');
	if (data.result.limited_time_offer_ends_on != null)
		$('#productDiv').append('<div><b>Sale ends</b>: ' + data.result.limited_time_offer_ends_on + '</div>');
	if (data.result.description != null)
		$('#productDiv').append('<div><b>Description</b>: ' + data.result.description + '</div>');
	if (data.result.alcohol_content != 0)
		$('#productDiv').append('<div><b>Alcohol %</b>: ' + data.result.alcohol_content / 100 + '%</div>');
	if (data.result.bonus_reward_miles != 0)
		$('#productDiv').append('<div><b>Bonus reward miles</b>: ' + data.result.bonus_reward_miles + '</div>');
	if (data.result.bonus_reward_miles_ends_on != null)
		$('#productDiv').append('<div><b>Bonus reward miles ends</b>: ' + data.result.bonus_reward_miles_ends_on + '</div>');
	
	ShowRightPane();
}
			
function GetProductsCallBack(data) 
{	
	ShowLeftPane();

	$('#prev').prop("disabled", false);
	$('#next').prop("disabled", false);
	
	if (data.pager.is_first_page == true)
		$('#prev').prop("disabled", true);
	else if (data.pager.is_final_page == true)
		$('#next').prop("disabled", true);
	
	$('#pageNumber').empty();
	$('#totalPages').empty();
	$('#pageNumber').append(data.pager.current_page);
	$('#totalPages').append(data.pager.final_page);				
	
	nextPage = data.pager.next_page;
	previousPage = data.pager.previous_page;
	
	$.each(data.result, function(i, item)
	{
		var className = 'oddRow';
		if (i % 2 == 0)
			className = 'evenRow';
			
		$('#results').append('<div id="product' + item.id + '" class=' + className + ' onClick="GetIndividualProduct(' + item.id + ')">' + 
			' <span class="productListItem">' + item.name + '</span><span="productListPrice> $' + item.price_in_cents/100 + '<span></div>');
	});
}

function GetPrevious()
{				
	GetProducts(previousPage);
}

function GetNext()
{				
	GetProducts(nextPage);
}

function HideLeftPane()
{
	$('#productsTitle').hide();
	$('#results').hide();
	$('#paging').hide();
}

function HideRightPane()
{
	$('#singleProductTitle').hide();
	$('#productDiv').hide();
}

function ShowLeftPane()
{
	$('#productsTitle').show();
	$('#results').show();
	$('#paging').show();
}

function ShowRightPane()
{
	$('#singleProductTitle').show();
	$('#productDiv').show();				
}		