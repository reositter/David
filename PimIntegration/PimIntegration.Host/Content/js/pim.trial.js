﻿$(document).ready(function () {

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
			$('.now').val(moment().format('YYYY-MM-DD hh:mm:ss'));
		});
	}

	$('#devtool-options').on('click', 'li > a', loadForm);
	
	$('#form-wrapper').on('click', '#btnGetProductByDateDummy', function (e) {
		e.preventDefault();
		displayInProgress();
		$.ajax({
			type: 'GET', url: '/trial/pim/getproductbydatedummy',
			data: {},
			success: displayJsonResponse
		});
	});
	
	$('#form-wrapper').on('click', '#btnSinceAction', function (e) {
		e.preventDefault();
		displayInProgress();

		var $btn = $(this);

		$.ajax({
			type: $btn.data('method'),
			url: $btn.data('url'),
			data: {
				Timestamp: $('#txtTimestamp').val()
			},
			success: displayJsonResponse
		});
	});

	$('#form-wrapper').on('click', '#btnGetProductBySku', function (e) {
		e.preventDefault();
		displayInProgress();
		$.ajax({
			type: 'GET', url: '/trial/pim/product/' + $('#txtSku').val(),
			data: { },
			success: displayJsonResponse
		});
	});
});