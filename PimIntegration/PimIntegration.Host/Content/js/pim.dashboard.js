$(document).ready(function() {
	$.ajax({
		type: 'GET', url: '/recentmessages/30',
		data: {},
		success: function(table) {
			$('#messagesWrapper').html(table);
		}
	});
});