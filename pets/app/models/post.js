var mongoose     = require('mongoose');
var Schema       = mongoose.Schema;

var PostSchema   = new Schema({
	creator: Schema.Types.ObjectId,
	album: Schema.Types.ObjectId,
	petId: Schema.Types.ObjectId,
	timestamp: {type: Date, default: Date.now},
	location: String,
	visibility: Number,
	comment_stream: Schema.Types.ObjectId,
	like_stream: Schema.Types.ObjectId,
	tag: Schema.Types.ObjectId
});

module.exports = mongoose.model('post', PostSchema);