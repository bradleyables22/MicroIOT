window.SystemGroupService = {
    baseUrl: '/api/v1/SystemGroups',

    async getAll() {
        const response = await fetch(this.baseUrl);
        if (!response.ok) throw new Error('Failed to fetch system groups.');
        return await response.json();
    },

    async getById(id) {
        const response = await fetch(`${this.baseUrl}/${id}`);
        if (!response.ok) throw new Error(`Failed to fetch system group with ID ${id}.`);
        return await response.json();
    },

    async create(systemGroup) {
        const response = await fetch(this.baseUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(systemGroup)
        });
        if (!response.ok) throw new Error('Failed to create system group.');
        return await response.json();
    },

    async update(id, systemGroup) {
        const response = await fetch(`${this.baseUrl}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(systemGroup)
        });
        if (!response.ok) throw new Error(`Failed to update system group with ID ${id}.`);
        return await response.json();
    },

    async delete(id) {
        const response = await fetch(`${this.baseUrl}/${id}`, {
            method: 'DELETE'
        });
        if (!response.ok) throw new Error(`Failed to delete system group with ID ${id}.`);
        return true;
    }
};