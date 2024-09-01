mergeInto(LibraryManager.library, {
	PrimeConnection: function(sender, socketURL, capacity){

		this.NetworkNode = Pointer_stringify(sender);
		var connection = new RTCMultiConnection();
		var jSocketURL = Pointer_stringify(socketURL);

		connection.socketURL = jSocketURL;
		
		var publicRoomIdentifier = 'islandbowling';
		connection.publicRoomIdentifier = publicRoomIdentifier;
		connection.socketMessageEvent = publicRoomIdentifier;
		
		connection.maxParticipantsAllowed = parseInt(capacity);

		connection.autoCreateMediaElement = false;
		connection.autoCloseEntireSession = true;
				
		connection.session = {
		  audio: false,
		  video: false,
		  data: true
		};

		connection.mediaConstraints = {
		  audio: true,
		  video: false
		};

		connection.sdpConstraints.mandatory = {
		  OfferToReceiveAudio: true,
		  OfferToReceiveVideo: false,
		  VoiceActivityDetection: false,
		  IceRestart: false
		};

		// https://www.rtcmulticonnection.org/docs/iceServers/
		// find non-google STUNs: 
		// https://raw.githubusercontent.com/pradt2/always-online-stun/master/valid_hosts.txt
		
		connection.candidates = {
			turn: true,
			stun: true
		};
		connection.iceTransportPolicy = 'all';
		
		connection.iceServers = [];
		
		connection.iceServers.push({
			urls: 'stun:stun.nexphone.ch:3478'
		});
		
		connection.iceServers.push({
			urls: 'stun:stun.threema.ch:3478'
		});

		connection.iceServers.push({
			urls: "turn:a.relay.metered.ca:80",
			username: "6b54310d192366596d5df7bc",
			credential: "gNhASBfoEeUWokJm",
		});

		connection.iceServers.push({
			urls: "turn:a.relay.metered.ca:80?transport=tcp",
			username: "6b54310d192366596d5df7bc",
			credential: "gNhASBfoEeUWokJm",
		});
		
		connection.iceServers.push({
			urls: "turn:a.relay.metered.ca:443",
			username: "fa4cd9890d762dfa224c98d8",
			credential: "ooCaXZjzUE9RC8sz",
		});

		connection.iceServers.push({
			urls: "turn:relay.metered.ca:443?transport=tcp",
			username: "fa4cd9890d762dfa224c98d8",
			credential: "ooCaXZjzUE9RC8sz",
		});

		connection.onopen = function(event) {
		  //console.log("a peer is born:" + JSON.stringify(event));
		  
		  // report the connection event to unity
		  SendMessage(NetworkNode, "OnConnectedToNetwork", JSON.stringify(event));
		};

		connection.onclose = function(event){
			console.log("disconnection event:\n" + JSON.stringify(event));       
			connection.disconnectWith(event.userid);
			connection.deletePeer(event.userid);
			
			// send event to unity
			SendMessage(NetworkNode, "OnDisconnectedFromNetwork", JSON.stringify(event));
		};

		connection.onleave = function(event) {
			Object.keys(connection.streamEvents).forEach(function(streamid) {
				var streamEvent = connection.streamEvents[streamid];
				if (streamEvent.userid === event.userid) {
					streamEvent.stream.getAudioTracks().forEach(function(track) {
						console.log ("stopping audio track");
						track.enabled = false;
					});
					connection.onstreamended(streamEvent);
				}
			});
		};
		
		connection.onRoomFull = function(roomid) {
			console.log('Room Full');
		};
		
		connection.onUserStatusChanged = function(event) {
			var targetUserId = event.userid;
			
			if (event.status == 'offline'){
				console.log(targetUserId + " left");
				SendMessage(NetworkNode, "RemoveAvatar", targetUserId);
			}
		};
		
		connection.onmessage = function(event) {
			SendMessage(NetworkNode, "ReceivePoseData", JSON.stringify(event));
		};
		
		this.connection = connection;
		SendMessage(NetworkNode, 'OnFinishedLoadingRTC', JSON.stringify(connection));
	},
	
    ConfigureAudio__deps: ['audioToAvatar'],
	ConfigureAudio: function() {
		
		var audioCtx = new AudioContext();
			
		connection.onstreamended = function(event) {
			console.log("stream ended:" + JSON.stringify(event));
			
			connection.streamEvents.selectAll({
				userid: event.userid,
				remote: true,
				isAudio: true
			}).forEach(function(streamEvent) {
				streamEvent.stream.stop();
			});
		};

		connection.onstream = function(event) {
			if (event.type === 'remote') {
				try{
					
				var audioEvent = event;
				var audioMimeType = 'audio/webm';

				// identify mediastream object
				var mediaStream = connection.streamEvents[event.streamid].stream;
				
				//console.log("OnStream::" + event.streamid + ",\n" + JSON.stringify(event.stream));
				
				// create media recorder 
				var options = { 
					mimeType: audioMimeType,
					audioBitsPerSecond: audioCtx.sampleRate
				}
				var mediaRecorder = new MediaRecorder(mediaStream, options);
				var audioBlob = null;
				var noChunks = 0;
				var header = null;
				
				// configure data callback
				mediaRecorder.ondataavailable = function (event) {
					noChunks++;
					if (noChunks == 1){
						audioBlob = new Blob([event.data], { 'type' : audioMimeType });
						header = audioBlob.slice(0, 264);
					}
					else{
						audioBlob = new Blob([header, event.data.slice(4,-1)], { 'type' : audioMimeType});
					}

					var audioURL = URL.createObjectURL(audioBlob); 
					audioEvent.mediaElement.URL = audioURL;
					_audioToAvatar(audioEvent);
				}
				
				mediaStream.onremovetrack = function(event) {
					console.log("Removed track: " + event.track.kind + ":" + event.track.label);
					mediaRecorder.stop();
					noChunks = 0;
				};
				
				// collect the stream
				mediaRecorder.start(1000);
				
				}
				catch(e){
					console.log("broken stream: " + e);
				}
			}
		};
		
		// end events
		SendMessage(NetworkNode, "OnAudioConfigured", "");
	},

    audioToAvatar : function(event){
			SendMessage(event.userid, "AddAudioStream", JSON.stringify(event));
	},
	
	function() {
		  var params = {},
			  r = /([^&=]+)=?([^&]*)/g;

		  function d(s) {
			  return decodeURIComponent(s.replace(/\+/g, ' '));
		  }
		  var match, search = window.location.search;
		  while (match = r.exec(search.substring(1)))
			  params[d(match[1])] = d(match[2]);
		  window.params = params;
		},

    OpenRoom: function(sender, roomId, roomSize, isPublic){
		try {
			// todo: use UTF8ToString instead of Pointer_stringify
			var roomManagerNode = Pointer_stringify(sender);
			var jRoomId = Pointer_stringify(roomId);

			// this doesn't seem to work (with or without 'this.' and 'parseInt')
			this.connection.maxParticipantsAllowed = parseInt(roomSize);

			connection.checkPresence(jRoomId, function (isRoomExist, roomid) {
				if (!isRoomExist) {
					connection.open(roomid, function (isRoomOpened, roomid, error) {
						if (error == 'Room full') {
							console.log("Room is full");
							SendMessage(roomManagerNode, "RoomIsFull", JSON.stringify(roomid));
							return;
						}
						SendMessage(roomManagerNode, "RoomCreated", roomid);
					});
				}
			});
		}		
		catch(e) {
			console.log("Error opening room:\n" + e);
		}
			
    },

	JoinRoom: function(sender, roomId) {
		var roomManagerNode = Pointer_stringify(sender);
		var jRoomId = Pointer_stringify(roomId);
		
		connection.checkPresence(jRoomId, function(isRoomExist, roomid){
			if (isRoomExist) {
				connection.join(roomid, function (isRoomJoined, roomid, error) {
					if (error == 'Room full') {
						console.log("Room is full");
						SendMessage(roomManagerNode, "RoomIsFull", JSON.stringify(roomid));
						return;
					}
					SendMessage(roomManagerNode, "RoomJoined", JSON.stringify(roomid));
				});
			}
		});
	},
	
	CeaseConnection : function(){
		
		// disconnect with all users
		connection.getAllParticipants().forEach(function(pid) {
			connection.disconnectWith(pid);
		});

		// stop all local cameras
		connection.attachStreams.forEach(function(localStream) {
			localStream.stop();
		});

		// close socket.io connection
		connection.closeSocket();
		SendMessage(NetworkNode, "ConnectionClosed", "");
	},

    SendData : function(params){
		try{
			var jMsg = Pointer_stringify(params);
			connection.send(jMsg);
		}
		catch(e){
			console.log("Error sending message\n params:" + stringify(params) + '\n' + e);
		}
	},
		
	createOffer: function(userId) {
		connection.createOffer().then(function(offer) {
			return connection.setLocalDescription(offer);
			})
			.then(function() {
				SendData({
					target: userId,
					type: "audio-offer",
					sdp: connection.localDescription
				});
			})
			.catch(function(reason) {
				console.log("offer creation failed:" + reason);
		});
	},

    StartAudioStream : function(userId){
		
		var jUserId = Pointer_stringify(userId);
		var foundStream = false;
		
		try{
			
		Object.keys(connection.streamEvents).forEach(function(streamid) {
			var event = connection.streamEvents[streamid];
			if (event.userid === jUserId && event.stream.isAudio) {
				foundStream = true;
				var pc = connection.peers[jUserId].peer;
				
				//console.log("unmuting stream to " + jUserId);
				//event.stream.unmute('audio');

				var micOptions = {
					audio: true,
					video: false
				};

				connection.captureUserMedia(function(microphone) {
					var streamEvent = {
						type: 'local',
						stream: microphone,
						streamid: microphone.id
					};
					connection.onstream(streamEvent);
					
					connection.attachStreams.forEach(function(localStream) {
						streamEvent.stream.getTracks().forEach(function(track) {
							console.log("adding new track:" + JSON.stringify(track));
							pc.addTrack(track, localStream);
						});
					});
		
				}, micOptions);
				

				console.log("renegotiating reconnection");
					
				connection.sdpConstraints.mandatory = {
					OfferToReceiveAudio: true,
					OfferToReceiveVideo: false,
					VoiceActivityDetection: false,
					IceRestart: true
				};

				connection.mediaConstraints = {
					video: false,
					audio: true
				};
				
				connection.peers[jUserId].addStream({
					data: true,
					audio: true,
					oneway: true
				});
				
				connection.renegotiate(jUserId);
			}
		});
		
		if (!foundStream){
			connection.session = {
				audio: true,
				video: false,
				data: true
			};
			
			connection.sdpConstraints.mandatory = {
				OfferToReceiveAudio: true,
				OfferToReceiveVideo: false,
				VoiceActivityDetection: false,
				IceRestart: false
			};

			connection.mediaConstraints = {
				video: false,
				audio: true
			};
				
			connection.peers[jUserId].addStream({
				data: true,
				audio: true,
				oneway: true
			});
		}
		
		}
		catch(e){
			console.log("error adding audio stream");
		}
		
		
    },

    StopAudioStream: function(userId){
		var jUserId = Pointer_stringify(userId);

		connection.streamEvents.selectAll({
			userid: jUserId,
			remote: true,
			isAudio: true
		}).forEach(function(streamEvent) {
			
			streamEvent.stream.getTracks().forEach(function(track) {
				track.stop();
			});
			
			streamEvent.stream.getTracks().forEach(function(track) {
				streamEvent.stream.removeTrack(track);
			});
		});
		
		connection.renegotiate(jUserId);
		
    },
	
	
	
});