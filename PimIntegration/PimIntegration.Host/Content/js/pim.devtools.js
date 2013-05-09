$(document).ready(function() {
	$('#btnGetNewProductsByDateDummy').on('click', function(e) {
		e.preventDefault();
		$.ajax({
			type: 'GET', url: 'products/new',
			data: {},
			success: function(products) {
				console.log(products);
				$('#response').html(jsontree(products));
			}
		});
	});
});