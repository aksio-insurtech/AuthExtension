// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

const viewObject = document.getElementById('devToolbarView');
const viewDocument = viewObject.contentDocument;
const view = viewDocument.getElementById('view');

if (view) {
    const tenantSelector = viewDocument.getElementById('tenantSelector');
    const identitySelector = viewDocument.getElementById('identitySelector');
    let identities = [];

    const getCookie = (name) => {
        const decoded = decodeURIComponent(document.cookie);
        const cookies = decoded.split(';');
        const cookie = cookies.find(_ => _.trim().indexOf(`${name}=`) == 0);
        if (cookie) {
            const keyValue = cookie.split('=');
            return [keyValue[0].trim(), keyValue[1].trim()];
        }
        return [];
    };

    const handleSelectionChanges = () => {
        const json = {
            tenant: tenantSelector.selectedOptions[0].value,
            identity: identitySelector.selectedOptions[0].value
        };

        document.cookie = `.aksio-dev=${JSON.stringify(json)}`;

        const identity = identities.find(_ => _.id == json.identity);
        const tidClaim = identity.claims.find(_ => _.typ === 'tid');
        if (tidClaim) {
            tidClaim.val = json.tenant;
        } else {
            identity.claims.push({
                typ: 'http://schemas.microsoft.com/identity/claims/tenantid',
                val: json.tenant
            });
        }
        document.cookie = `aksiodevprincipalid=${identity.id}`;
        document.cookie = `aksiodevprincipalname=${identity.name}`;
        document.cookie = `aksiodevprincipal=${btoa(JSON.stringify(identity))}`;
        document.cookie = ".aksio-identity=;expires="+ new Date(0).toUTCString();
        location.reload();
    };

    const setSelections = () => {
        const cookie = getCookie('.aksio-dev');
        if (cookie.length == 2) {
            const json = JSON.parse(cookie[1]);
            for (const option of tenantSelector.options) {
                if (option.value == json.tenant) {
                    option.selected = 'selected';
                }
            }

            for (const option of identitySelector.options) {
                if (option.value == json.identity) {
                    option.selected = 'selected';
                }
            }
        } else {
            handleSelectionChanges();
        }
    };

    document.body.appendChild(view);
    viewObject.remove();
    document.body.style.marginTop = "20px";

    (async () => {
        const cratisResponse = await fetch('/.aksio/dev/cratis.json');
        const cratisConfig = await cratisResponse.json();

        for (const tenantKey in cratisConfig.tenants) {
            const tenant = cratisConfig.tenants[tenantKey];
            const option = document.createElement('option');
            option.value = tenantKey;
            option.innerText = tenant.name;
            tenantSelector.appendChild(option);
        }

        const identitiesResponse = await fetch('/.aksio/dev/identities.json');
        identities = await identitiesResponse.json();

        for (const identity of identities) {
            const option = document.createElement('option');
            option.value = identity.id;
            option.innerText = identity.name;
            identitySelector.append(option);
        }

        setSelections();
    })();

    tenantSelector.onchange = () => {
        handleSelectionChanges();
    };

    identitySelector.onchange = () => {
        handleSelectionChanges();
    };
}
