$(document).ready(function() {
	$.ajax({
		type: 'GET', url: '/recentrequests/30',
		data: {},
		success: function(table) {
			$('#messagesWrapper').html(table);
		}
	});
});