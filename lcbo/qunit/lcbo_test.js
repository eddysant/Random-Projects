test( "lcboSetup", function() {
	lcboSetup();
	
	ok( !is_visible('#singleProductTitle'), 'singleProductTitle hidden');
	ok( !is_visible('#productDiv'), 'productDiv hidden');	
	ok( !is_visible('#productsTitle'), 'productsTitle hidden'); 
	ok( !is_visible('#results'), 'results hidden'); 
	ok( !is_visible('#paging'), 'paging hidden'); 	
});	
	
test( "GenerateProductsUrl_NULL", function() {
	var url = GenerateProductsUrl(null);
	ok( url == "http://lcboapi.com/products?page=null&where_not=is_discontinued&callback=?", "GenerateProductsUrl_NULL");
});

test( "GenerateProductsUrl_1", function() {
	var url = GenerateProductsUrl(1);
	ok( url == "http://lcboapi.com/products?page=1&where_not=is_discontinued&callback=?", "GenerateProductsUrl_1");
});

test( "GenerateProductsUrl_airmilesCheck", function() {
	$('#airmilesCheck').prop('checked',true);
	var url = GenerateProductsUrl(1);
	ok( url == "http://lcboapi.com/products?page=1&where_not=is_discontinued&where=has_bonus_reward_miles&callback=?", "GenerateProductsUrl_airmilesCheck");
});

test( "GenerateProductsUrl_promotionCheck", function() {
	$('#promotionCheck').prop('checked',true);
	var url = GenerateProductsUrl(1);
	ok( url == "http://lcboapi.com/products?page=1&where_not=is_discontinued&where=has_value_added_promotion&callback=?", "GenerateProductsUrl_promotionCheck");
});

test( "GenerateIndividualProductUrl_NULL", function() {
	var url = GenerateIndividualProductUrl(null);
	ok( url == "http://lcboapi.com/products/null", "GenerateIndividualProductUrl_NULL");
});

test( "GenerateIndividualProductUrl_1", function() {
	var url = GenerateIndividualProductUrl(1);
	ok( url == "http://lcboapi.com/products/1", "GenerateIndividualProductUrl_1");
});

test( "GenerateIndividualProductUrl_1", function() {
	var url = GenerateIndividualProductUrl(1);
	ok( url == "http://lcboapi.com/products/1", "GenerateIndividualProductUrl_1");
});

test( "NewSearch", function() {
	GetProducts = function(n) {
		ok( n == 1, "NewSearch = 1");	
	};	
	NewSearch();	
});

test( "GetProductsCallBack", function() {	
	var data = {'pager': { 'is_first_page' : true, "current_page": "1", "final_page": "500", "next_page":2, "previous_page":0 }, 'result' : { 'id': 1, 'name' : 'Steamwhistle', 'price_in_cents': 250}};
	ok ( $('#results').html() == "",  "results == ''");
	GetProductsCallBack(data);		
	
	ok( is_visible('#productsTitle'), 'productsTitle visible'); 
	ok( is_visible('#results'), 'results visible'); 
	ok( is_visible('#paging'), 'paging visible'); 		
	ok ( $('#pageNumber').text() == "1",  "pageNumber == 1");
	ok ( $('#totalPages').text() == "500",  "totalPages == 500");
	ok ( $('#results').html() != "",  "results != ''");
	ok ( nextPage == 2,  "nextPage == 2");
	ok ( previousPage == 0,  "previousPage == 0");
});

test( "GetIndividualProductCallBack", function() {	
	var data = {'result' : { 'name' : 'Steamwhistle',  'primary_category': 'Beer', 'price_in_cents': 250, 'regular_price_in_cents': 250, 'alcohol_content': 500, 'bonus_reward_miles': 0} };
	GetIndividualProductCallBack(data);		
	ok ( $('#singleProductTitle').text() == "Steamwhistle",  "text == Steamwhistle");
	ok ( $('#productDiv').html() == "<div><b>Category</b>: Beer</div><div><b>Price</b>: $2.5</div><div><b>Regular price</b>: $2.5</div><div><b>Alcohol %</b>: 5%</div>",  "productDiv == ''");
	ok( is_visible('#singleProductTitle'), 'singleProductTitle visible');
	ok( is_visible('#productDiv'), 'productDiv visible');
});

test( "GetPrevious", function() {
	GetProducts = function(n) {
		ok( n == 5, "GetPrevious = 5");	
	};	
	previousPage = 5;
	GetPrevious();	
});

test( "GetNext", function() {
	GetProducts = function(n) {
		ok( n == 7, "GetNext = 7");	
	};	
	previousPage = 7;
	GetPrevious();	
});

test( "ErrorMessage", function() {
	var data = {'message': "error54321"};
	ErrorMessage(data);  
	ok( $('#error').text() == "error54321"); 
})

test( "HideLeftPane", function() {
	HideLeftPane();  
	ok( !is_visible('#productsTitle'), 'productsTitle hidden'); 
	ok( !is_visible('#results'), 'results hidden'); 
	ok( !is_visible('#paging'), 'paging hidden'); 
})

test( "HideRightPane", function() {
	HideRightPane();
	ok( !is_visible('#singleProductTitle'), 'singleProductTitle hidden');
	ok( !is_visible('#productDiv'), 'productDiv hidden');	
})

test( "ShowLeftPane", function() {
	ShowLeftPane();  
	ok( is_visible('#productsTitle'), 'productsTitle visible'); 
	ok( is_visible('#results'), 'results visible'); 
	ok( is_visible('#paging'), 'paging visible'); 	
})

test( "ShowRightPane", function() {
	ShowRightPane();  
	ok( is_visible('#singleProductTitle'), 'singleProductTitle visible');
	ok( is_visible('#productDiv'), 'productDiv visible');
})

function is_visible(id)
{	
	return $(id).is(":visible");
}