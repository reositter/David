$(document).ready(function () {
	function displayJsonResponse(json) {
		$('#response').html(jsontree(json));
		$('#response > ul').first().treeview();
	}

	$('#lnkGetNewProductsByDate').on('click', function(e) {
		e.preventDefault();
		$.get('devtools/method/newproducts', function(markup) {
			$('#form-wrapper').html(markup);
		});
	});

	$('#form-wrapper').on('click', '#btnGetNewProductsSince', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/new',
			data: {
				Hour: $('#txtHour').val(),
				Minute: $('#txtMinute').val()
			},
			success: displayJsonResponse
		});
	});

	$('#btnGetNewProductsByDateDummy').on('click', function(e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/new',
			data: {},
			success: displayJsonResponse
		});
	});

	$('#btnGetArticlesForPriceUpdate').on('click', function(e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/forpriceupdate',
			data: {},
			success: displayJsonResponse
		});
	});
	
	$('#btnGetArticlesForPriceUpdateAndCalcAgreedPrice').on('click', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/forpriceupdatewithagreedprice',
			data: {},
			success: displayJsonResponse
		});
	});
	
	$('#btnGetStockBalanceQuery').on('click', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/forstockbalanceupdate',
			data: {},
			success: displayJsonResponse
		});
	});
});