'use strict';

var express = require('express')
  , app = express()
  ;

app.configure(function () {
  app.set('view engine', 'jade');
  app.use(express.static(__dirname + '/public'));
  app.use(express.errorHandler());
});

app.listen(9001);
console.log('listening on 9001');