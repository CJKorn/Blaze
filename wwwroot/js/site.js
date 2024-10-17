function initMap() {
    var map = L.map('map').setView([-33.865143, 151.209900], 10); // Coordinates for Sydney
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);
    var latlngs = [[37, -109.05], [41, -109.03], [41, -102.05], [37, -102.04]];
    var polygon = L.polygon(latlngs, { color: 'red' }).addTo(map);
    //map.fitBounds(polygon.getBounds());
}
