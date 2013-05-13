$(document).ready(function () {
	function displayJsonResponse(json) {
		$('#response').html(jsontree(json));
		$('#response > ul').first().treeview({
			collapsed: false
		});
	}

	function loadForm(e) {
		e.preventDefault();
		var href = $(this).attr('href');
		$.get(href, function (markup) {
			$('#form-wrapper').html(markup);
			$('#response').empty();
		});
	}

	$('#devtool-options').on('click', 'li > a', loadForm);

	$('#form-wrapper').on('click', '#btnGetProductByDate', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/new/getproductbydate',
			data: {
				Hour: $('#txtHour').val(),
				Minute: $('#txtMinute').val(),
				Second: $('#txtSecond').val()
			},
			success: displayJsonResponse
		});
	});

	$('#form-wrapper').on('click', '#btnGetNewProductsTask', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'POST', url: '/products/getnewproductstask',
			data: {
				Hour: $('#txtHour').val(),
				Minute: $('#txtMinute').val(),
				Second: $('#txtSecond').val()
			},
			success: displayJsonResponse
		});
	});
	
	$('#form-wrapper').on('click', '#btnGetProductByDateDummy', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: '/products/new/getproductbydatedummy',
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