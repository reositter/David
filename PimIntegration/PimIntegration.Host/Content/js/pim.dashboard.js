$(document).ready(function() {
	$.ajax({
		type: 'GET', url: '/recentrequests/30',
		data: {},
		success: function(table) {
			$('#logWrapper').html(table);
		}
	});

	$('#logWrapper').on('click', '.lnkRequestItem, .lnkResponseItem', function (e) {
		e.preventDefault();
		$.ajax({
			type: 'GET',
			url: $(this).attr('href'),
			success: function(json) {
				console.dir(eval(json));
			},
			error: function(jXhr, textStatus, errorThrown) {
				console.dir(jXhr);
				console.log(textStatus);
				console.log(errorThrown);
			}
		});
	});
});