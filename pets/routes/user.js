var express           = require('express'),
    UsersController   = express.Router();

var user    		  = require('../app/models/user');
		
UsersController.route('/')
  .get(function(req, res) {
		user.find(function(err, users) {
			if (err)
				res.send(err);

			res.json(users);
		});
	})
	
  .post(function(req, res) {
		
		var newuser = new user();		// create a new instance of the user model
		newuser.visibility = 1;
		newuser.first_name = req.body.first_name;
		newuser.last_name = req.body.last_name;
		newuser.location = req.body.location;
		newuser.email = req.body.email;
		newuser.flogin = req.body.flogin;
		newuser.ilogin = req.body.ilogin;
		
		//newuser.petIds
		//newuser.feed
		//newuser.following
		//newuser.followers
		
		newuser.save(function(err) {
			if (err)
				res.send(err);

			res.json({ message: 'User created!' });
		});

		
	});

UsersController.route('/:user_id')

	// get the user with that id
	.get(function(req, res) {
		user.findById(req.params.user_id, function(err, user) {
			if (err)
				res.send(err);
			res.json(user);
		});
	})

	// update the user with this id
	.put(function(req, res) {
		user.update({_id:req.params.user_id},req.body)
			.then(function(success){
				res.json(success);
			})
			.catch(function (error) {
				res.status(404).send(err);
			});	
	})

	// delete the user with this id
	.delete(function(req, res) {
		user.remove({
			_id: req.params.user_id
		}, function(err, user) {
			if (err)
				res.send(err);

			res.json({ message: 'Successfully deleted' });
		});
	});

module.exports = UsersController;