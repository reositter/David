$(document).ready(function () {

	function displayInProgress() {
		$('#response').html('Waiting...');
	}

	function displayJsonResponse(json) {
		if (!json || ($.isArray(json) && json.length == 0)) {
			$('#response').html('Empty result');
		}
		else {
			$('#response').html(jsontree(json));
			$('#response > ul').first().treeview({
				collapsed: true
			});
		}
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
	
	$('#form-wrapper').on('click', '#btnGetProductByDateDummy', function (e) {
		e.preventDefault();
		displayInProgress();
		$.ajax({
			type: 'GET', url: '/products/new/getproductbydatedummy',
			data: {},
			success: displayJsonResponse
		});
	});

	$('#form-wrapper').on('click', '#btnGetProductByDate', function (e) {
		e.preventDefault();
		displayInProgress();
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
		displayInProgress();
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
	
	$('#form-wrapper').on('click', '#btnGetArticlesForStockBalanceUpdate', function (e) {
		e.preventDefault();
		displayInProgress();
		$.ajax({
			type: 'GET', url: 'products/forstockbalanceupdate',
			data: {
				Hour: $('#txtHour').val(),
				Minute: $('#txtMinute').val(),
				Second: $('#txtSecond').val()
			},
			success: displayJsonResponse
		});
	});
	
	$('#form-wrapper').on('click', '#btnPublishStockBalanceUpdatesTask', function (e) {
		e.preventDefault();
		displayInProgress();
		$.ajax({
			type: 'POST', url: '/products/publishstockbalanceupdatestask',
			data: {
				Hour: $('#txtHour').val(),
				Minute: $('#txtMinute').val(),
				Second: $('#txtSecond').val()
			},
			success: displayJsonResponse
		});
	});

	$('#form-wrapper').on('click', '#btnGetArticlesForPriceUpdate', function (e) {
		e.preventDefault();
		displayInProgress();
		$.ajax({
			type: 'GET', url: 'products/forpriceupdate',
			data: {},
			success: displayJsonResponse
		});
	});

});