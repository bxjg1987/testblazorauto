export async function requestPort() {
    let r = await navigator.serial.requestPort();
    await r.open({ baudRate: 9600 /* pick your baud rate */ });
    r =  r.getInfo();
    console.log(r);
    return r;
}

export async function getPorts() {
    return navigator.serial.getPorts();
}