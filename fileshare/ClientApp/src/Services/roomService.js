async function getRooms() {
    const response = await fetch('room');
    return await response.json();
}

async function loginToRoom(roomId, password) {
    await fetch(`room/${roomId}/login`, { method: "POST", body: { password }});
}

async function createRoom(roomName, password) {
    await fetch('room', {method: "POST", body: {name: roomName, password: password}});
}

export { getRooms, loginToRoom };