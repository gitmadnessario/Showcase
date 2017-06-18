var mongoose     = require('mongoose');
var Schema       = mongoose.Schema;

var PetSchema   = new Schema({
	name: String,
	race: String,
	color: String,
	album: Schema.Types.ObjectId
});

module.exports = mongoose.model('pet', PetSchema);