socket.on('testing:test', function (data) {
    socket.broadcast.emit('send:message', {
      user: name,
      text: data.message
    });
  });