$(document).ready(function() {
	$.ajax({
		type: 'GET', url: '/lostmessages/20',
		data: {},
		success: function(table) {
			$('#lostMessagesWrapper').html(table);
		}
	});
});