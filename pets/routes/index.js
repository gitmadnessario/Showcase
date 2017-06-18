var _         = require('lodash'),
    fs        = require('fs'),
    excluded  = ['index.js'];
	tree	  = ['pet.js','post.js']

module.exports = function(app) {
  fs.readdirSync(__dirname).forEach(function(file) {
    // Remove extension from file name
    var basename = file.split('.')[0];
	var flag = 0;
	
	if (_.includes(tree,file)){
		flag = 1;
	}
	
    // Only load files that aren't directories and aren't blacklisted
    if (!fs.lstatSync(__dirname + '/' + file).isDirectory() && !_.includes(excluded, file)) {
	  if (flag == 1){
		app.use('/:userId/' + basename, require('./' + file) );
	  }
	  else{
		app.use('/', require('./' + file));
	  }
    }
  });
};
