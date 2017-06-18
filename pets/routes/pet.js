var express           = require('express'),
    PetsController   = express.Router();

var pet		 = require('../app/models/pet');
var user     = require('../app/models/user');
	
PetsController.route('/?')
  .get(function(req, res) {
		console.log(req.originalUrl);
		var splitted = req.originalUrl.split("/",2);
		var user_id = splitted[1];
		pet.find(function(err, pets) {
			if (err)
				res.send(err);

			res.json(pets);
		});
	})
	
  .post(function(req, res) {
		console.log(req.originalUrl);
		var splitted = req.originalUrl.split("/",2);
		var user_id = splitted[1];
		var newpet = new pet();		// create a new instance of the pet model
		newpet.name = req.body.name;
		newpet.race = req.body.race;
		newpet.color = req.body.color;
		//newpet.album = req.body.album;
		
		user.findOne({_id: user_id}, function(err,user,newpet){
			if(err)
				res.send(err);
			console.log(user.email);
			console.log(newpet.color);
			//user.feed.push(newpet.id);
		});

		newpet.save(function(err) {
			if (err)
				res.send(err);

			res.json({ message: 'Pet created!' });
		});	
	});

PetsController.route('/?/:pet_id')

	// get the pet with that id
	.get(function(req, res) {
		pet.findById(req.params.pet_id, function(err, pet) {
			if (err)
				res.send(err);
			res.json(pet);
		});
	})

	// update the pet with this id
	.put(function(req, res) {
		user.update({_id:req.params.pet_id},req.body)
			.then(function(success){
				res.json(success);
			})
			.catch(function (error) {
				res.status(404).send(err);
			});	
	})

	// delete the pet with this id
	.delete(function(req, res) {
		pet.remove({
			_id: req.params.pet_id
		}, function(err, pet) {
			if (err)
				res.send(err);

			res.json({ message: 'Successfully deleted' });
		});
	});

module.exports = PetsController;