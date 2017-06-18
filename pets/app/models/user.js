var mongoose     = require('mongoose');
var Schema       = mongoose.Schema;

var UserSchema   = new Schema({
	petIds: [Schema.Types.ObjectId],
	visibility: Number,
	feed: [{ type: Schema.Types.ObjectId, ref: 'feed'}],
	following: [Schema.Types.ObjectId],
	followers: [Schema.Types.ObjectId],
	first_name: String,
	last_name: String,
	location: String,
	email: String,
	flogin: String,
	ilogin: String
});

module.exports = mongoose.model('user', UserSchema);