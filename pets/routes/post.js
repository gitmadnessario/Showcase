var express           = require('express'),
    PostController   = express.Router();

var post     = require('../app/models/post');
var user     = require('../app/models/user');
		
PostController.route('/?')
  .get(function(req, res) {
		post.find(function(err, posts) {
			if (err)
				res.send(err);

			res.json(posts);
		});
	})
	
  .post(function(req, res) {
		
		var newpost = new post();		// create a new instance of the post model
		newpost.visibility = 1;
		newpost.location = req.body.location;
		
		//newpost.creator
		//newpost.album
		//newpost.petId
		//newpost.comment_stream
		//newpost.like_stream
		//newpost.tag
		
		newpost.save(function(err) {
			if (err)
				res.send(err);

			res.json({ message: 'Post created!' });
		});

		
	});

PostController.route('/?/:post_id')

	// get the post with that id
	.get(function(req, res) {
		post.findById(req.params.post_id, function(err, post) {
			if (err)
				res.send(err);
			res.json(post);
		});
	})

	// update the post with this id
	.put(function(req, res) {
		post.update({_id:req.params.post_id},req.body)
			.then(function(success){
				res.json(success);
			})
			.catch(function (error) {
				res.status(404).send(err);
			});	
	})

	// delete the post with this id
	.delete(function(req, res) {
		post.remove({
			_id: req.params.post_id
		}, function(err, post) {
			if (err)
				res.send(err);

			res.json({ message: 'Successfully deleted' });
		});
	});

module.exports = PostController;